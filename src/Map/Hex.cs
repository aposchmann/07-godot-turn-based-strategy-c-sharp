using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

public class Hex
{
    public Vector2I Coordinates { get; init; }

    public Terrain Terrain { get; init; }
}