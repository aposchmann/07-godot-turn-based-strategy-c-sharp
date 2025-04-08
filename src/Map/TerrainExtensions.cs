namespace de.nodapo.turnbasedstrategygame.Map;

public static class TerrainExtensions
{
    public static (int Row, int Column) ToTileMap(this Terrain terrain) => ((int)terrain / 2, (int)terrain % 2);
}