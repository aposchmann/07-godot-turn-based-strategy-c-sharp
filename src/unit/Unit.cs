using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.civilization;
using Godot;
using static Godot.MouseButtonMask;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Unit : Node2D
{
    private Civilization? _civilization;

    private Sprite2D? _image;
    private Area2D? _imageArea;

    private bool _isSelected;

    public static IReadOnlyDictionary<Type, PackedScene> UnitScenes { get; } = new Dictionary<Type, PackedScene>
    {
        { typeof(Settler), GD.Load<PackedScene>("res://src/unit/Settler.tscn") },
        { typeof(Warrior), GD.Load<PackedScene>("res://src/unit/Warrior.tscn") }
    };

    public static IReadOnlyDictionary<Type, Texture2D> UnitTextures { get; } = new Dictionary<Type, Texture2D>
    {
        { typeof(Settler), GD.Load<Texture2D>("res://textures/unit/settler_image.png") },
        { typeof(Warrior), GD.Load<Texture2D>("res://textures/unit/warrior_image.jpg") }
    };

    public int ProductionRequired { get; protected set; }
    public string UnitName { get; protected set; } = null!;

    public Vector2I Coordinates { get; set; }

    public Civilization? Civilization
    {
        set
        {
            if (_civilization == value) return;

            _civilization?.Units.Remove(this);
            _civilization = value;

            if (_civilization == null) return;

            _civilization.Units.Add(this);
            Image.Modulate = _civilization.TerritoryColor;
        }
    }

    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int MaxMoves { get; protected set; }
    public int CurrentMoves { get; protected set; }

    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;

            _isSelected = value;

            Image.Modulate = new Color(Image.Modulate)
            {
                V = Image.Modulate.V + (_isSelected ? -0.25f : 0.25f)
            };
        }
    }

    private Sprite2D Image => _image ??= GetNode<Sprite2D>("Image");
    private Area2D ImageArea => _imageArea ??= GetNode<Area2D>("Image/Area");

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton { ButtonMask: Left }) return;

        var intersectionResult = GetWorld2D().DirectSpaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            CollideWithAreas = true,
            Position = GetGlobalMousePosition()
        });

        if (intersectionResult.Count > 0 && (Area2D)intersectionResult[0]["collider"] == ImageArea)
        {
            IsSelected = true;
            GetViewport().SetInputAsHandled();
        }
        else
        {
            IsSelected = false;
        }
    }
}