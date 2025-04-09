using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static de.nodapo.turnbasedstrategygame.Map.Terrain;
using static Godot.FastNoiseLite.FractalTypeEnum;

namespace de.nodapo.turnbasedstrategygame.Map;

public partial class HexMap : Node2D
{
    [Export] public int Width = 42;
    [Export] public int Height = 42;

    private TileMapLayer? _baseLayer;
    private TileMapLayer? _borderLayer;
    private TileMapLayer? _overlayLayer;

    private TileMapLayer BaseLayer =>
        _baseLayer ??= GetNode<TileMapLayer>("BaseLayer") ?? throw new NullReferenceException();

    private TileMapLayer BorderLayer =>
        _borderLayer ??= GetNode<TileMapLayer>("BorderLayer") ?? throw new NullReferenceException();

    private TileMapLayer OverlayLayer =>
        _overlayLayer ??= GetNode<TileMapLayer>("OverlayLayer") ?? throw new NullReferenceException();

    private readonly Dictionary<Vector2I, Hex> _hexes = new();

    public override void _Ready()
    {
        GenerateTerrain();
    }

    public Vector2 ToLocal(Vector2I coordinates) => BaseLayer.MapToLocal(coordinates);

    private void GenerateTerrain()
    {
        var random = new Random();
        var seed = random.Next(int.MaxValue);

        var baseTerrain = GenerateBaseTerrain(seed);

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var coordinates = new Vector2I(x, y);

                var hex = new Hex
                {
                    Coordinates = coordinates,
                    Terrain = baseTerrain.NoiseRanges
                        .First(range =>
                            baseTerrain.NoiseValues[x, y] >= range.Min &&
                            baseTerrain.NoiseValues[x, y] < range.Max)
                        .terrain
                };

                _hexes[coordinates] = hex;

                BaseLayer.SetCell(coordinates, 0, hex.Terrain.ToTileMapLayerCoordinates());
                BorderLayer.SetCell(coordinates, 0, new Vector2I(0, 0));
            }
        }
    }

    private (float[,] NoiseValues, List<(float Min, float Max, Terrain terrain)> NoiseRanges)
        GenerateBaseTerrain(int seed)
    {
        var noise = new FastNoiseLite();

        noise.Seed = seed;
        noise.Frequency = 0.008f;
        noise.FractalType = Fbm;
        noise.FractalOctaves = 4;
        noise.FractalLacunarity = 2.25f;

        var noiseValues = new float[Width, Height];
        var noiseMax = 0f;

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                noiseValues[x, y] = Math.Abs(noise.GetNoise2D(x, y));

                if (noiseValues[x, y] > noiseMax) noiseMax = noiseValues[x, y];
            }
        }

        var ranges = new List<(float Min, float Max, Terrain terrain)>
        {
            (0, noiseMax / 10 * 2.5f, Water),
            (noiseMax / 10 * 2.5f, noiseMax / 10 * 4.0f, Coast),
            (noiseMax / 10 * 4.0f, noiseMax / 10 * 4.5f, Beach),
            (noiseMax / 10 * 4.5f, noiseMax + 0.05f, Plains)
        };

        return (noiseValues, ranges);
    }
}