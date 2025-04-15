using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

public static class TerrainExtensions
{
    /// <summary>
    ///     Converts a Terrain enum value to tile map layer coordinates.
    ///     Each terrain type maps to a specific position in a 2-column grid.
    /// </summary>
    /// <param name="terrain">The terrain enum value to convert</param>
    /// <returns>Vector2I with column (X) and row (Y) coordinates</returns>
    public static Vector2I ToTileMapLayerCoordinates(this Terrain terrain)
    {
        var row = (int)terrain / 2;
        var column = (int)terrain % 2;

        return new Vector2I(column, row);
    }
}