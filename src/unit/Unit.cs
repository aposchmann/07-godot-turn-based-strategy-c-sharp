using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.civilization;
using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Unit : Node2D
{
    public static Dictionary<Type, PackedScene> UnitScenes { get; } = new()
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
            if (value == null) return;

            value.Units.Add(this);
            ImageSprite.Modulate = value.TerritoryColor;
        }
    }

    private Sprite2D? _imageSprite;

    private Sprite2D ImageSprite => _imageSprite ??= GetNode<Sprite2D>("Image");
}