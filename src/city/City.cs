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

    private Civilization? _civilization;

    private Sprite2D? _imageSprite;

    private Label? _nameLabel;

    private List<Hex> _populationGrowthHexPool = [];

    private int _populationGrowthThreshold = 10;

    private int _populationGrowthTracker;

    public Unit? UnitBeingBuilt { get; set; }

    public int UnitBuildTracker { get; private set; }

    public HexMap? HexMap { get; set; }

    public Vector2I CenterCoordinates { get; set; }

    public List<Hex> Territory { get; } = [];

    public int Population { get; private set; } = 1;

    public int TotalFood { get; private set; }

    public int TotalProduction { get; private set; }

    public List<Unit> UnitBuildQueue { get; } = [];

    public Civilization? Civilization
    {
        get => _civilization;
        set
        {
            if (_civilization == value) return;

            if (_civilization != null)
            {
                _civilization.Cities.Remove(this);
                HexMap?.UpdateCivilizationTerritory(_civilization);
            }

            _civilization = value;

            if (_civilization == null) return;

            _civilization.Cities.Add(this);
            ImageSprite.Modulate = _civilization.TerritoryColor;
            HexMap?.UpdateCivilizationTerritory(_civilization);
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

        if (_populationGrowthTracker >= _populationGrowthThreshold)
        {
            Population++;

            _populationGrowthTracker %= _populationGrowthThreshold;
            _populationGrowthThreshold += PopulationThresholdIncrease;

            AddNeighborHexToTerritory();
        }

        ProcessUnitBuildQueue();
    }

    private void ProcessUnitBuildQueue()
    {
        UnitBuildTracker += TotalProduction;

        while (UnitBuildQueue.Count > 0)
        {
            UnitBeingBuilt ??= UnitBuildQueue[0];

            if (UnitBeingBuilt.ProductionRequired > UnitBuildTracker) return;

            SpawnUnit(UnitBeingBuilt);

            UnitBuildTracker -= UnitBeingBuilt.ProductionRequired;

            UnitBuildQueue.RemoveAt(0);
            UnitBeingBuilt = null;
        }
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
        if (Civilization?.MaximumUnits > Civilization?.Units.Count)
        {
            UnitBuildQueue.Add(unit);
        }
    }

    private void SpawnUnit(Unit unit)
    {
        if (HexMap == null) return;

        var unitToSpawn = Unit.UnitScenes[unit.GetType()].Instantiate<Unit>();

        unitToSpawn.Position = HexMap.ToLocal(CenterCoordinates);
        unitToSpawn.Civilization = Civilization;
        unitToSpawn.Coordinates = CenterCoordinates;

        HexMap.AddChild(unitToSpawn);
    }
}