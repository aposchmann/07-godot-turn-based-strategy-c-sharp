using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

public static class TerrainExtensions
{
    public static Vector2I ToTileMap(this Terrain terrain) => new((int)terrain / 2, (int)terrain % 2);
}