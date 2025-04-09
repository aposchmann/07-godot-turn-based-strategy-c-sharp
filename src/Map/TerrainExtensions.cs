using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

public static class TerrainExtensions
{
    public static Vector2I ToTileMapLayerCoordinates(this Terrain terrain)
    {
        var row = (int)terrain / 2;
        var column = (int)terrain % 2;

        return new Vector2I(column, row);
    }
}