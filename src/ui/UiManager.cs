using System;
using Godot;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class UiManager : Node2D
{
    private PackedScene? _terrainPanelScene;

    private PackedScene TerrainPanelScene => _terrainPanelScene ??=
        GD.Load<PackedScene>("res://ui/TerrainPanel.tscn") ?? throw new NullReferenceException();
}