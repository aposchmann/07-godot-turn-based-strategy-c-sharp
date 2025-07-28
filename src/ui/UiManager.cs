using de.nodapo.turnbasedstrategygame.city;
using de.nodapo.turnbasedstrategygame.map;
using de.nodapo.turnbasedstrategygame.terrain;
using Godot;
using static Godot.ResourceLoader;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class UiManager : Node2D
{
    private CityPanel? _cityPanel;
    private PackedScene? _cityPanelScene;
    private GeneralPanel? _generalPanel;
    private HexMap? _hexMap;

    private TerrainPanel? _terrainPanel;

    private PackedScene? _terrainPanelScene;

    private PackedScene TerrainPanelScene => _terrainPanelScene
        ??= Load<PackedScene>("res://src/terrain/TerrainPanel.tscn");

    private PackedScene CityPanelScene => _cityPanelScene
        ??= Load<PackedScene>("res://src/city/CityPanel.tscn");

    private HexMap HexMap => _hexMap ??= GetNode<HexMap>("/root/Game/HexMap");
    private GeneralPanel GeneralPanel => _generalPanel ??= GetNode<GeneralPanel>("GeneralPanel");

    public override void _Ready()
    {
        HexMap.HexSelected += OnHexSelected;
    }

    public override void _ExitTree()
    {
        HexMap.HexSelected -= OnHexSelected;
    }

    public void HidePanels()
    {
        if (_terrainPanel != null)
        {
            RemoveChild(_terrainPanel);

            _terrainPanel.QueueFree();
            _terrainPanel = null;
        }

        if (_cityPanel != null)
        {
            RemoveChild(_cityPanel);

            _cityPanel.QueueFree();
            _cityPanel = null;
        }
    }

    public void ProcessEndTurn()
    {
        HexMap.ProcessEndTurn();

        _cityPanel?.Refresh();
    }

    private void OnHexSelected(object? _, HexSelectedEventArgs hexSelectedEventArgs)
    {
        HidePanels();

        var hex = hexSelectedEventArgs.Hex;

        if (hex is { IsCityCenter: true, OwnerCity: not null })
        {
            _cityPanel = CityPanelScene.Instantiate<CityPanel>();

            AddChild(_cityPanel);

            _cityPanel.SetCity(hex.OwnerCity);
        }
        else
        {
            _terrainPanel = TerrainPanelScene.Instantiate<TerrainPanel>();

            AddChild(_terrainPanel);

            _terrainPanel.SetHex(hex);
        }
    }
}