using System;
using de.nodapo.turnbasedstrategygame.Map;
using de.nodapo.turnbasedstrategygame.Terrain;
using Godot;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class UiManager : Node2D
{
    private TerrainPanel? _terrainPanel;
    private PackedScene? _terrainPanelScene;

    private PackedScene TerrainPanelScene => _terrainPanelScene ??=
        ResourceLoader.Load<PackedScene>("res://src/terrain/TerrainPanel.tscn") ?? throw new NullReferenceException();

    public override void _Ready()
    {
        GetNode<HexMap>("/root/Game/HexMap").HexSelected += OnHexSelected;
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