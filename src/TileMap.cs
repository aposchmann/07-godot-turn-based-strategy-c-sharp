using Godot;

namespace de.nodapo.turnbasedstrategygame;

public partial class TileMap : Node2D
{
    [Export] public int Width = 100;
    [Export] public int Height = 60;

    private TileMapLayer _baseLayer = null!;
    private TileMapLayer _borderLayer = null!;
    private TileMapLayer _overlayLayer = null!;

    public override void _Ready()
    {
        _baseLayer = GetNode<TileMapLayer>("BaseLayer");
        _borderLayer = GetNode<TileMapLayer>("BorderLayer");
        _overlayLayer = GetNode<TileMapLayer>("OverlayLayer");

        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _baseLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
                _borderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
        }
    }
}