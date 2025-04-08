using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

public class Hex(Vector2I coordinates)
{
    public readonly Vector2I Coordinates = coordinates;

    private Terrain Terrain { get; set; }
}