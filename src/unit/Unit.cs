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

    public int maxHealth { get; set; }
    public int currentHealth { get; protected set; }
    public int maxMoves { get; set; }
    public int currentMoves { get; protected set; }

    private Sprite2D ImageSprite => _imageSprite ??= GetNode<Sprite2D>("Image");
}