using System;
using de.nodapo.turnbasedstrategygame.Map;
using de.nodapo.turnbasedstrategygame.Terrain;
using Godot;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class UiManager : Node2D
{
    private PackedScene? _terrainPanelScene;

    private TerrainPanel? _terrainPanel;

    private PackedScene TerrainPanelScene => _terrainPanelScene ??=
        ResourceLoader.Load<PackedScene>("res://src/terrain/TerrainPanel.tscn") ?? throw new NullReferenceException();

    public void SetTerrainPanel(Hex hex)
    {
        _terrainPanel?.QueueFree();

        _terrainPanel = TerrainPanelScene.Instantiate<TerrainPanel>();

        AddChild(_terrainPanel);

        _terrainPanel.SetHex(hex);
    }
}