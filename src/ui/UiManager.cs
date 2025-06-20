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
        GetNode<HexMap>("/root/Game/HexMap").SelectedHexChanged += SetTerrainPanel;
    }

    private void SetTerrainPanel(object? _, Hex hex)
    {
        if (_terrainPanel != null)
        {
            RemoveChild(_terrainPanel);

            _terrainPanel.QueueFree();
        }

        _terrainPanel = TerrainPanelScene.Instantiate<TerrainPanel>();

        AddChild(_terrainPanel);

        _terrainPanel.SetHex(hex);
    }
}