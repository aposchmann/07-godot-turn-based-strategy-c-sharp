using System;
using System.Collections.Generic;
using System.Linq;
using de.nodapo.turnbasedstrategygame.civilization;
using de.nodapo.turnbasedstrategygame.map;
using de.nodapo.turnbasedstrategygame.terrain;
using de.nodapo.turnbasedstrategygame.unit;
using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class City : Node2D
{
    private const int PopulationThresholdIncrease = 20;
    private readonly Random _random = new();

    private string? _cityName;

    private Sprite2D? _imageSprite;

    private Label? _nameLabel;

    private List<Hex> _populationGrowthHexPool = [];

    private int _populationGrowthThreshold = 10;

    private int _populationGrowthTracker;

    public Unit? UnitBeingBuilt;

    public int UnitBuildTracker;

    public HexMap? HexMap { get; set; }

    public Vector2I CenterCoordinates { get; set; }

    public List<Hex> Territory { get; } = [];

    public int Population { get; private set; } = 1;

    public int TotalFood { get; private set; }

    public int TotalProduction { get; private set; }

    public List<Unit> UnitBuildQueue { get; } = [];

    public Civilization? Civilization
    {
        set
        {
            if (value != null)
                ImageSprite.Modulate = value.TerritoryColor;
        }
    }

    public string? CityName
    {
        get => _cityName;
        set
        {
            _cityName = value;
            NameLabel.Text = value;
        }
    }

    private Sprite2D ImageSprite => _imageSprite ??= GetNode<Sprite2D>("Image");
    private Label NameLabel => _nameLabel ??= GetNode<Label>("Name");

    public void ProcessEndTurn()
    {
        _populationGrowthTracker += TotalFood;

        if (_populationGrowthTracker < _populationGrowthThreshold) return;

        Population++;
        _populationGrowthTracker %= _populationGrowthThreshold;
        _populationGrowthThreshold += PopulationThresholdIncrease;

        AddNeighborHexToTerritory();
    }

    public void AddTerritory(List<Hex> hexes)
    {
        hexes
            .Where(hex => hex.OwnerCity == null)
            .ToList()
            .ForEach(hex =>
            {
                hex.OwnerCity = this;

                Territory.Add(hex);

                AddNeighborsToBorderPool(hex);

                _populationGrowthHexPool.Remove(hex);
            });

        CalculateTerritoryResourceTotals();
    }

    private void CalculateTerritoryResourceTotals()
    {
        TotalFood = Territory.Sum(hex => hex.Food);
        TotalProduction = Territory.Sum(hex => hex.Production);
    }

    private void AddNeighborsToBorderPool(Hex hex)
    {
        _populationGrowthHexPool.AddRange(HexMap?
            .GetSurroundingHexes(hex.Coordinates)
            .Where(neighborHex => !_populationGrowthHexPool.Contains(neighborHex))
            .Where(neighborHex => !new[]
            {
                Terrain.Water, Terrain.Ice, Terrain.Coast, Terrain.Mountain
            }.Contains(neighborHex.Terrain))
            .Where(neighborHex => neighborHex.OwnerCity == null) ?? []);
    }

    private void AddNeighborHexToTerritory()
    {
        _populationGrowthHexPool = _populationGrowthHexPool.Where(hex => hex.OwnerCity == null).ToList();

        if (_populationGrowthHexPool.Count == 0) return;

        AddTerritory([_populationGrowthHexPool[_random.Next(_populationGrowthHexPool.Count)]]);
    }

    public void AddUnitToBuildQueue(Unit unit)
    {
        UnitBuildQueue.Add(unit);
    }
}