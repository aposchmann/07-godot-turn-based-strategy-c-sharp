using de.nodapo.turnbasedstrategygame.Map;
using de.nodapo.turnbasedstrategygame.Terrain;
using Godot;
using static Godot.ResourceLoader;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class UiManager : Node2D
{
    private HexMap? _hexMap;
    private TerrainPanel? _terrainPanel;
    private PackedScene? _terrainPanelScene;

    private PackedScene TerrainPanelScene =>
        _terrainPanelScene ??= Load<PackedScene>("res://src/terrain/TerrainPanel.tscn");

    private HexMap HexMap => _hexMap ??= GetNode<HexMap>("/root/Game/HexMap");

    public override void _Ready()
    {
        HexMap.HexSelected += OnHexSelected;
    }

    public override void _ExitTree()
    {
        HexMap.HexSelected -= OnHexSelected;
    }

    public void HideTerrainPanel()
    {
        _terrainPanel?.QueueFree();
        _terrainPanel = null;
    }

    private void OnHexSelected(object? _, HexSelectedEventArgs hexSelectedEventArgs)
    {
        if (_terrainPanel == null)
        {
            _terrainPanel = TerrainPanelScene.Instantiate<TerrainPanel>();

            AddChild(_terrainPanel);
        }

        _terrainPanel.SetHex(hexSelectedEventArgs.Hex);
    }
}