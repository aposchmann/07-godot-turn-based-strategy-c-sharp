using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using de.nodapo.turnbasedstrategygame.Terrain;
using Godot;
using static de.nodapo.turnbasedstrategygame.Terrain.Terrain;
using static Godot.FastNoiseLite.FractalTypeEnum;
using static Godot.FastNoiseLite.NoiseTypeEnum;
using static Godot.MouseButtonMask;

namespace de.nodapo.turnbasedstrategygame.Map;

using Terrain = Terrain.Terrain;

public partial class HexMap : Node2D
{
    private readonly Dictionary<Vector2I, Hex> _hexes = new();

    private TileMapLayer? _baseLayer;
    private TileMapLayer? _borderLayer;
    private TileMapLayer? _overlayLayer;

    private Vector2I? _selectedHex;
    [Export] public int Height = 60;
    [Export] public int Width = 100;

    private TileMapLayer BaseLayer => _baseLayer ??=
        GetNode<TileMapLayer>("BaseLayer") ?? throw new NullReferenceException();

    private TileMapLayer BorderLayer => _borderLayer ??=
        GetNode<TileMapLayer>("BorderLayer") ?? throw new NullReferenceException();

    private TileMapLayer OverlayLayer => _overlayLayer ??=
        GetNode<TileMapLayer>("OverlayLayer") ?? throw new NullReferenceException();

    public event EventHandler<Hex>? SelectedHexChanged;

    public override void _Ready()
    {
        GenerateTerrain();
        GenerateResources();
    }

    private void GenerateResources()
    {
        var random = new Random();

        foreach (var hex in _hexes.Values)
            switch (hex.Terrain)
            {
                case Plains:
                    hex.Food = random.Next(2, 6);
                    hex.Production = random.Next(0, 3);
                    break;
                case Desert:
                    hex.Food = random.Next(0, 2);
                    hex.Production = random.Next(0, 2);
                    break;
                case Beach:
                    hex.Food = random.Next(0, 4);
                    hex.Production = random.Next(0, 2);
                    break;
                case Forest:
                    hex.Food = random.Next(1, 4);
                    hex.Production = random.Next(2, 6);
                    break;
                case Water:
                case Mountain:
                case Coast:
                case Ice:
                    // NOOP
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        const int maxIce = 3;

        Parallel.For(0, Width, x =>
        {
            var northIce = random.Next(maxIce + 1);
            var southIce = random.Next(maxIce + 1);

            for (var y = 0; y <= northIce; y++)
                CreateIce(x, y);

            for (var y = Height - 1; y >= Height - southIce - 1; y--)
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

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton { ButtonMask: Left }) return;

        var clickedPosition = OverlayLayer.LocalToMap(ToLocal(GetGlobalMousePosition()));

        if (clickedPosition == _selectedHex) return;

        DeselectHex();

        if (!_hexes.TryGetValue(clickedPosition, out var clickedHex)) return;

        SelectHex(clickedPosition);

        GD.Print(clickedHex);

        SelectedHexChanged?.Invoke(this, clickedHex);
    }

    private void SelectHex(Vector2I hexPosition)
    {
        OverlayLayer.SetCell(hexPosition, 0, new Vector2I(0, 1));

        _selectedHex = hexPosition;
    }

    private void DeselectHex()
    {
        if (_selectedHex is not { } selectedHex) return;

        OverlayLayer.SetCell(selectedHex);

        _selectedHex = null;
    }
}