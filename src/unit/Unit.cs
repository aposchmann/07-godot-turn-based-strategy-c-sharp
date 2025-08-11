using System;
using System.Collections.Generic;
using System.Linq;
using de.nodapo.turnbasedstrategygame.civilization;
using de.nodapo.turnbasedstrategygame.map;
using de.nodapo.turnbasedstrategygame.terrain;
using de.nodapo.turnbasedstrategygame.ui;
using Godot;
using static Godot.MouseButtonMask;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Unit : Node2D
{
    private static readonly Dictionary<Hex, List<Unit>> UnitLocations = [];
    private Civilization? _civilization;

    private HexMap? _hexMap;
    private Sprite2D? _image;
    private Area2D? _imageArea;

    private bool _isSelected;

    private UiManager? _uiManager;

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

    private static IReadOnlySet<Terrain> ImpassibleTerrain { get; } = new HashSet<Terrain>
    {
        Terrain.Coast, Terrain.Ice, Terrain.Mountain, Terrain.Water
    };

    public int ProductionRequired { get; protected set; }
    public string UnitName { get; protected set; } = null!;

    public Vector2I Coordinates { get; set; }
    protected HexMap HexMap => _hexMap ??= GetNode<HexMap>("/root/Game/HexMap");

    public Civilization? Civilization
    {
        protected get => _civilization;
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

            if (_isSelected) UiManager.OnUnitSelected(this);
        }
    }

    private UiManager UiManager => _uiManager ??= GetNode<UiManager>("/root/Game/CanvasLayer/UiManager");
    private Sprite2D Image => _image ??= GetNode<Sprite2D>("Image");
    private Area2D ImageArea => _imageArea ??= GetNode<Area2D>("Image/Area");

    private List<Unit> CurrentUnitLocations => GetUnitLocationsAt(HexMap.GetHex(Coordinates));

    private void OnHexRightClicked(object? _, HexRightClickedEventArgs eventArgs) => Move(eventArgs.Hex);

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

    private List<Hex> CalculateValidMovementHexes()
    {
        return HexMap.GetSurroundingHexes(Coordinates).Where(hex => !ImpassibleTerrain.Contains(hex.Terrain)).ToList();
    }

    public override void _Ready()
    {
        CurrentUnitLocations.Add(this);

        HexMap.HexRightClicked += OnHexRightClicked;
    }

    private void Move(Hex hex)
    {
        if (!IsSelected) return;
        if (CurrentMoves < 1) return;
        if (!CalculateValidMovementHexes().Contains(hex)) return;
        if (GetUnitLocationsAt(hex).Count > 0) return;

        CurrentUnitLocations.Remove(this);

        Coordinates = hex.Coordinates;

        Position = HexMap.ToLocal(Coordinates);

        CurrentUnitLocations.Add(this);

        CurrentMoves--;

        UiManager.OnUnitSelected(this);
    }

    private static List<Unit> GetUnitLocationsAt(Hex hex)
    {
        var unitList = UnitLocations.GetValueOrDefault(hex, []);

        UnitLocations[hex] = unitList;

        return unitList;
    }

    public void ProcessEndTurn()
    {
        CurrentMoves = MaxMoves;
    }

    protected void DestroyUnit()
    {
        IsSelected = false;

        HexMap.HexRightClicked -= OnHexRightClicked;

        Civilization?.Units.Remove(this);

        GetUnitLocationsAt(HexMap.GetHex(Coordinates)).Remove(this);

        QueueFree();
    }
}