using System.Collections.Generic;
using Godot;
using static System.Single;
using static Godot.Input;

namespace de.nodapo.turnbasedstrategygame;

public partial class Camera : Camera2D
{
    [Export] public int Velocity = 20;
    [Export] public float ZoomSpeed = 0.05f;

    private const float ZoomMin = 0.1f;
    private const float ZoomMax = 3.0f;

    private Dictionary<string, (int X, int Y)> KeyboardMovements => new()
    {
        { "map_right", (Velocity, 0) },
        { "map_left", (-Velocity, 0) },
        { "map_up", (0, -Velocity) },
        { "map_down", (0, Velocity) }
    };

    private Dictionary<string, float> KeyboardZooms => new()
    {
        { "map_zoom_in", ZoomSpeed },
        { "map_zoom_out", -ZoomSpeed }
    };

    private Dictionary<string, float> MouseZooms => new()
    {
        { "mouse_zoom_in", ZoomSpeed },
        { "mouse_zoom_out", -ZoomSpeed }
    };

    private float _leftBound, _rightBound, _topBound, _bottomBound;

    private TileMap? _tileMap;

    private TileMap TileMap => _tileMap ??= GetNode<TileMap>("../TileMap");

    public override void _Ready()
    {
        _leftBound = ToGlobal(TileMap.ToLocal(new Vector2I(0, 0))).X + 100;
        _rightBound = ToGlobal(TileMap.ToLocal(new Vector2I(TileMap.Width, 0))).X - 100;
        _topBound = ToGlobal(TileMap.ToLocal(new Vector2I(0, 0))).Y + 50;
        _bottomBound = ToGlobal(TileMap.ToLocal(new Vector2I(0, TileMap.Height))).Y - 50;
    }

    public override void _PhysicsProcess(double delta)
    {
        var zoom = 0f;

        foreach (var key in KeyboardMovements.Keys)
        {
            if (!IsActionPressed(key)) continue;

            var (x, y) = KeyboardMovements[key];

            Position = new Vector2(
                Clamp(Position.X + x, _leftBound, _rightBound),
                Clamp(Position.Y + y, _topBound, _bottomBound));
        }

        foreach (var key in KeyboardZooms.Keys)
        {
            if (!IsActionPressed(key)) continue;

            zoom += KeyboardZooms[key];
        }

        foreach (var key in MouseZooms.Keys)
        {
            if (!IsActionJustReleased(key)) continue;

            zoom += MouseZooms[key];
        }

        Zoom = new Vector2
        {
            X = Clamp(Zoom.X + zoom, ZoomMin, ZoomMax),
            Y = Clamp(Zoom.Y + zoom, ZoomMin, ZoomMax)
        };
    }
}