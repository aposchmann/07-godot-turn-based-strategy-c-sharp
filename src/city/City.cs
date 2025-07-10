using System.Collections.Generic;
using System.Linq;
using de.nodapo.turnbasedstrategygame.civilization;
using de.nodapo.turnbasedstrategygame.map;
using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class City : Node2D
{
    private const int PopulationThresholdIncrease = 15;

    public static readonly Dictionary<Hex, City> InvalidTiles = new();

    private string? _cityName;

    private Civilization? _civilization;

    private Sprite2D? _imageSprite;

    private Label? _nameLabel;

    public List<Hex> BorderTilePool = [];

    public HexMap? HexMap { get; set; }

    public Vector2I CenterCoordinates { get; set; }

    public List<Hex> Territory { get; } = [];

    public int Population { get; private set; } = 1;

    public int PopulationGrowthThreshold { get; set; } = 10;
    public int PopulationGrowthTracker { get; set; }

    public int TotalFood { get; private set; }

    public int TotalProduction { get; private set; }

    public Civilization? Civilization
    {
        get => _civilization;
        set
        {
            _civilization = value;

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
        PopulationGrowthTracker += TotalFood;

        if (PopulationGrowthTracker >= PopulationGrowthThreshold)
        {
            Population++;
            PopulationGrowthTracker %= PopulationGrowthThreshold;
            PopulationGrowthThreshold += PopulationThresholdIncrease;
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
            });

        CalculateTerritoryResourceTotals();
    }

    private void CalculateTerritoryResourceTotals()
    {
        TotalFood = Territory.Sum(hex => hex.Food);
        TotalProduction = Territory.Sum(hex => hex.Production);
    }
}