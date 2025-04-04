using System.Collections.Generic;
using System.Linq;
using Godot;
using static System.Single;

namespace de.nodapo.turnbasedstrategygame;

public partial class Camera : Camera2D
{
    [Export] public int Velocity = 20;
    [Export] public float ZoomSpeed = 0.05f;

    private Dictionary<string, (int X, int Y)> MapMovements => new()
    {
        { "map_right", (Velocity, 0) },
        { "map_left", (-Velocity, 0) },
        { "map_up", (0, -Velocity) },
        { "map_down", (0, Velocity) }
    };

    private Dictionary<string, float> MapZooms => new()
    {
        { "map_zoom_in", ZoomSpeed },
        { "map_zoom_out", -ZoomSpeed }
    };

    public override void _PhysicsProcess(double delta)
    {
        MapMovements.Keys
            .Where(key => Input.IsActionPressed(key))
            .Select(key => MapMovements[key])
            .ToList()
            .ForEach(mapMovement => Position += new Vector2(mapMovement.X, mapMovement.Y));

        MapZooms.Keys
            .Where(key => Input.IsActionPressed(key))
            .Select(key => MapZooms[key])
            .ToList()
            .ForEach(mapZoom => Zoom = new Vector2
            {
                X = Clamp(Zoom.X + mapZoom, 0.1f, 3.0f),
                Y = Clamp(Zoom.Y + mapZoom, 0.1f, 3.0f)
            });
    }
}