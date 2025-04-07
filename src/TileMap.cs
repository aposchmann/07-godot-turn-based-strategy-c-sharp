using Godot;

namespace de.nodapo.turnbasedstrategygame;

public partial class TileMap : Node2D
{
    [Export] public int Width = 100;
    [Export] public int Height = 60;

    private TileMapLayer? _baseLayer;
    private TileMapLayer? _borderLayer;
    private TileMapLayer? _overlayLayer;

    private TileMapLayer BaseLayer => _baseLayer ??= GetNode<TileMapLayer>("BaseLayer");
    private TileMapLayer BorderLayer => _borderLayer ??= GetNode<TileMapLayer>("BorderLayer");
    private TileMapLayer OverlayLayer => _overlayLayer ??= GetNode<TileMapLayer>("OverlayLayer");

    public override void _Ready()
    {
        GenerateTerrain();
    }

    public Vector2 ToLocal(Vector2I coordinates) => BaseLayer.MapToLocal(coordinates);

    private void GenerateTerrain()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                BaseLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
                BorderLayer.SetCell(new Vector2I(x, y), 0, new Vector2I(0, 0));
            }
        }
    }
}