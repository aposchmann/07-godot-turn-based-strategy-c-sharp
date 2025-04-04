using System.Collections.Generic;
using System.Linq;
using Godot;
using static System.Single;

namespace de.nodapo.turnbasedstrategygame;

public partial class Camera : Camera2D
{
    [Export] public int Velocity = 20;
    [Export] public float ZoomSpeed = 0.05f;

    private const float ZoomMin = 0.1f;
    private const float ZoomMax = 3.0f;

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

    private string? _mouseWheelAction;

    public override void _PhysicsProcess(double delta)
    {
        _mouseWheelAction =
            Input.IsActionJustReleased("mouse_zoom_in") ? "map_zoom_in" :
            Input.IsActionJustReleased("mouse_zoom_out") ? "map_zoom_out" :
            null;

        MapMovements.Keys
            .Where(key => Input.IsActionPressed(key))
            .Select(key => MapMovements[key])
            .ToList()
            .ForEach(mapMovement => Position += new Vector2(mapMovement.X, mapMovement.Y));

        MapZooms.Keys
            .Where(key => Input.IsActionPressed(key) || key.Equals(_mouseWheelAction))
            .Select(key => MapZooms[key])
            .ToList()
            .ForEach(mapZoom => Zoom = new Vector2
            {
                X = Clamp(Zoom.X + mapZoom, ZoomMin, ZoomMax),
                Y = Clamp(Zoom.Y + mapZoom, ZoomMin, ZoomMax)
            });
    }
}