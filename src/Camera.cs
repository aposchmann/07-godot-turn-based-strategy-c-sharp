using System.Collections.Generic;
using System.Linq;
using Godot;

namespace de.nodapo.turnbasedstrategygame;

public partial class Camera : Camera2D
{
    [Export] public int Velocity = 20;
    [Export] public float ZoomSpeed = 0.05f;

    private Dictionary<string, (int X, int Y)> Movements => new()
    {
        { "map_right", (Velocity, 0) },
        { "map_left", (-Velocity, 0) },
        { "map_up", (0, -Velocity) },
        { "map_down", (0, Velocity) }
    };

    public override void _PhysicsProcess(double delta)
    {
        Movements.Keys
            .Where(key => Input.IsActionPressed(key))
            .Select(key => Movements[key])
            .ToList()
            .ForEach(movement => Position += new Vector2I(movement.X, movement.Y));
    }
}