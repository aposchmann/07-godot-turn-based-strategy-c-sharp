using Godot;

namespace de.nodapo.turnbasedstrategygame.Map;

using Terrain = Terrain.Terrain;

public class Hex
{
    public Vector2I Coordinates { get; init; }

    public Terrain Terrain { get; set; }

    public int Food { get; set; }

    public int Production { get; set; }

    public override string ToString()
    {
        return $"{Coordinates} {Terrain} Food: {Food} Production: {Production}";
    }
}