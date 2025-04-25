using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using de.nodapo.turnbasedstrategygame.Terrain;
using Godot;
using static de.nodapo.turnbasedstrategygame.Terrain.Terrain;
using static Godot.FastNoiseLite.FractalTypeEnum;
using static Godot.FastNoiseLite.NoiseTypeEnum;

namespace de.nodapo.turnbasedstrategygame.Map;

using Terrain = Terrain.Terrain;

public partial class HexMap : Node2D
{
    private readonly Dictionary<Vector2I, Hex> _hexes = new();

    private TileMapLayer? _baseLayer;
    private TileMapLayer? _borderLayer;
    private TileMapLayer? _overlayLayer;

    [Export] public int Height = 60;
    [Export] public int Width = 100;

    private TileMapLayer BaseLayer =>
        _baseLayer ??= GetNode<TileMapLayer>("BaseLayer") ?? throw new NullReferenceException();

    private TileMapLayer BorderLayer =>
        _borderLayer ??= GetNode<TileMapLayer>("BorderLayer") ?? throw new NullReferenceException();

    private TileMapLayer OverlayLayer =>
        _overlayLayer ??= GetNode<TileMapLayer>("OverlayLayer") ?? throw new NullReferenceException();

    public override void _Ready()
    {
        GenerateTerrain();
    }

    public Vector2 ToLocal(Vector2I coordinates)
    {
        return BaseLayer.MapToLocal(coordinates);
    }

    private void GenerateTerrain()
    {
        var random = new Random();
        var seed = random.Next(int.MaxValue);

        var baseNoise = GenerateBaseNoise(seed);
        var forestNoise = GenerateForestNoise(seed);
        var desertNoise = GenerateDesertNoise(seed);
        var mountainNoise = GenerateMountainNoise(seed);

        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var coordinates = new Vector2I(x, y);

            var terrain = GetTerrainFromNoise(baseNoise.Ranges, baseNoise.Values[x, y]) ?? Plains;

            if (terrain == Plains && GetTerrainFromNoise(
                    desertNoise.Ranges,
                    desertNoise.Values[x, y]) is { } desertTerrain)
                terrain = desertTerrain;

            if (terrain == Plains && GetTerrainFromNoise(
                    forestNoise.Ranges,
                    forestNoise.Values[x, y]) is { } forestTerrain)
                terrain = forestTerrain;

            if (terrain == Plains && GetTerrainFromNoise(
                    mountainNoise.Ranges,
                    mountainNoise.Values[x, y]) is { } mountainTerrain)
                terrain = mountainTerrain;

            var hex = new Hex
            {
                Coordinates = coordinates,
                Terrain = terrain
            };

            _hexes[coordinates] = hex;

            BaseLayer.SetCell(coordinates, 0, hex.Terrain.ToTileMapLayerCoordinates());
            BorderLayer.SetCell(coordinates, 0, new Vector2I(0, 0));
        }

        GenerateIceCaps(random);
    }

    private void GenerateIceCaps(Random random)
    {
        const int maxIce = 4;

        Parallel.For(0, Width, x =>
        {
            var northIce = random.Next(maxIce + 1);
            var southIce = random.Next(maxIce + 1);

            for (var y = 0; y < northIce; y++)
                CreateIce(x, y);

            for (var y = Height - 1; y >= Height - southIce; y--)
                CreateIce(x, y);
        });
    }

    private void CreateIce(int x, int y)
    {
        var coordinates = new Vector2I(x, y);
        var hex = _hexes[coordinates];

        hex.Terrain = Ice;

        BaseLayer.SetCell(coordinates, 0, hex.Terrain.ToTileMapLayerCoordinates());
    }

    private static Terrain? GetTerrainFromNoise(
        List<(float Min, float Max, Terrain Terrain)> noiseRanges,
        float noiseValue)
    {
        return noiseRanges
            .FirstOrDefault(range => noiseValue >= range.Min && noiseValue < range.Max)
            .Terrain;
    }

    private (float[,] Values, List<(float Min, float Max, Terrain Terrain)> Ranges)
        GenerateBaseNoise(int seed)
    {
        var (noiseValues, noiseMax) = new TerrainNoiseBuilder(Width, Height)
            .WithSeed(seed)
            .WithFrequency(0.008f)
            .WithFractalType(Fbm)
            .WithFractalLacunarity(2.25f)
            .WithFractalOctaves(4)
            .Build();

        return (noiseValues, [
            (0, noiseMax / 10 * 2.5f, Water),
            (noiseMax / 10 * 2.5f, noiseMax / 10 * 4.0f, Coast),
            (noiseMax / 10 * 4.0f, noiseMax / 10 * 4.5f, Beach),
            (noiseMax / 10 * 4.5f, noiseMax, Plains)
        ]);
    }

    private (float[,] Values, List<(float Min, float Max, Terrain Terrain)> Ranges)
        GenerateForestNoise(int seed)
    {
        var (noiseValues, noiseMax) = new TerrainNoiseBuilder(Width, Height)
            .WithSeed(seed)
            .WithNoiseType(Cellular)
            .WithFrequency(0.04f)
            .WithFractalType(Fbm)
            .WithFractalLacunarity(2f)
            .Build();

        return (noiseValues, [(noiseMax / 10 * 7.5f, noiseMax, Forest)]);
    }

    private (float[,] Values, List<(float Min, float Max, Terrain Terrain)> Ranges)
        GenerateDesertNoise(int seed)
    {
        var (noiseValues, noiseMax) = new TerrainNoiseBuilder(Width, Height)
            .WithSeed(seed)
            .WithNoiseType(SimplexSmooth)
            .WithFrequency(0.015f)
            .WithFractalType(Fbm)
            .WithFractalLacunarity(2f)
            .Build();

        return (noiseValues, [(noiseMax / 10 * 7.5f, noiseMax, Desert)]);
    }

    private (float[,] Values, List<(float Min, float Max, Terrain Terrain)> Ranges)
        GenerateMountainNoise(int seed)
    {
        var (noiseValues, noiseMax) = new TerrainNoiseBuilder(Width, Height)
            .WithSeed(seed)
            .WithNoiseType(Simplex)
            .WithFrequency(0.02f)
            .WithFractalType(Ridged)
            .WithFractalLacunarity(2f)
            .Build();

        return (noiseValues, [(noiseMax / 10 * 6.5f, noiseMax, Mountain)]);
    }
}