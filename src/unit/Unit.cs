using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.civilization;
using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Unit : Node2D
{
    private Civilization? _civilization;

    private Sprite2D? _imageSprite;

    public static IReadOnlyDictionary<Type, PackedScene> UnitScenes { get; } = new Dictionary<Type, PackedScene>
    {
        { typeof(Settler), GD.Load<PackedScene>("res://src/unit/Settler.tscn") },
        { typeof(Warrior), GD.Load<PackedScene>("res://src/unit/Warrior.tscn") }
    };

    public static IReadOnlyDictionary<Type, Texture2D> UnitTextures { get; } = new Dictionary<Type, Texture2D>
    {
        { typeof(Settler), GD.Load<Texture2D>("res://textures/unit/settler_image.png") },
        { typeof(Warrior), GD.Load<Texture2D>("res://textures/unit/warrior_image.png") }
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
            ImageSprite.Modulate = _civilization.TerritoryColor;
        }
    }

    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int MaxMoves { get; protected set; }
    public int CurrentMoves { get; protected set; }

    private Sprite2D ImageSprite => _imageSprite ??= GetNode<Sprite2D>("Image");
}