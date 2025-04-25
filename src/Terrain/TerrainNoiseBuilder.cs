using System;
using Godot;
using static Godot.FastNoiseLite;
using static Godot.FastNoiseLite.FractalTypeEnum;
using static Godot.FastNoiseLite.NoiseTypeEnum;

namespace de.nodapo.turnbasedstrategygame.Terrain;

public class TerrainNoiseBuilder(int width, int height)
{
    private float _fractalLacunarity = 2f;
    private int _fractalOctaves = 5;
    private FractalTypeEnum _fractalType = Fbm;
    private float _frequency;
    private NoiseTypeEnum _noiseType = SimplexSmooth;
    private int _seed;

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

        var noiseValues = new float[width, height];
        var noiseMax = 0f;

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            noiseValues[x, y] = Math.Abs(noise.GetNoise2D(x, y));
            noiseMax = Math.Max(noiseValues[x, y], noiseMax);
        }

        return (noiseValues, noiseMax);
    }
}