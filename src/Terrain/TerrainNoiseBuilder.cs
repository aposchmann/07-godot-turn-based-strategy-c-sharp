using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using static System.ArgumentOutOfRangeException;
using static Godot.FastNoiseLite;
using static Godot.FastNoiseLite.FractalTypeEnum;
using static Godot.FastNoiseLite.NoiseTypeEnum;

namespace de.nodapo.turnbasedstrategygame.terrain;

public class TerrainNoiseBuilder
{
    private readonly int _height;
    private readonly int _width;

    private float _fractalLacunarity = 2f;
    private int _fractalOctaves = 5;
    private FractalTypeEnum _fractalType = Fbm;
    private float _frequency = 0.01f;
    private NoiseTypeEnum _noiseType = SimplexSmooth;
    private int _seed;

    public TerrainNoiseBuilder(int width, int height)
    {
        ThrowIfNegative(width);
        ThrowIfNegative(height);

        _width = width;
        _height = height;
    }

    public TerrainNoiseBuilder WithSeed(int seed)
    {
        _seed = seed;
        return this;
    }

    public TerrainNoiseBuilder WithNoiseType(NoiseTypeEnum noiseType)
    {
        _noiseType = noiseType;
        return this;
    }

    public TerrainNoiseBuilder WithFrequency(float frequency)
    {
        _frequency = frequency;
        return this;
    }

    public TerrainNoiseBuilder WithFractalType(FractalTypeEnum fractalType)
    {
        _fractalType = fractalType;
        return this;
    }

    public TerrainNoiseBuilder WithFractalLacunarity(float fractalLacunarity)
    {
        _fractalLacunarity = fractalLacunarity;
        return this;
    }

    public TerrainNoiseBuilder WithFractalOctaves(int fractalOctaves)
    {
        _fractalOctaves = fractalOctaves;
        return this;
    }

    public (float[,] Values, float NoiseMax) Build()
    {
        var noise = new FastNoiseLite
        {
            Seed = _seed,
            NoiseType = _noiseType,
            Frequency = _frequency,
            FractalType = _fractalType,
            FractalLacunarity = _fractalLacunarity,
            FractalOctaves = _fractalOctaves
        };

        var noiseValues = new float[_width, _height];

        Parallel.For(0, _width, x =>
        {
            for (var y = 0; y < _height; y++) noiseValues[x, y] = Math.Abs(noise.GetNoise2D(x, y));
        });

        var noiseMax = noiseValues.Cast<float>().Max();

        return (noiseValues, noiseMax);
    }
}