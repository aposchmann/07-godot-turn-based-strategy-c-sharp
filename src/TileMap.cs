using System;
using Godot;

namespace de.nodapo.turnbasedstrategygame;

public partial class TileMap : Node2D
{
    [Export] public int Width = 100;
    [Export] public int Height = 60;

    private TileMapLayer? _baseLayer;
    private TileMapLayer? _borderLayer;
    private TileMapLayer? _overlayLayer;

    public override void _Ready()
    {
        _baseLayer = GetNode<TileMapLayer>("BaseLayer") ?? throw new NullReferenceException();
        _borderLayer = GetNode<TileMapLayer>("BorderLayer") ?? throw new NullReferenceException();
        _overlayLayer = GetNode<TileMapLayer>("OverlayLayer") ?? throw new NullReferenceException();

        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _baseLayer?.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
                _borderLayer?.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
        }
    }
}