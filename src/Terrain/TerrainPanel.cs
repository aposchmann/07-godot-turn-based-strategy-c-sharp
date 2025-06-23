using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.Map;
using Godot;
using static de.nodapo.turnbasedstrategygame.Terrain.Terrain;
using static Godot.ResourceLoader;

namespace de.nodapo.turnbasedstrategygame.Terrain;

public partial class TerrainPanel : Panel
{
    private Label? _foodLabel;
    private Label? _productionLabel;

    private TextureRect? _terrainImage;

    private Label? _terrainLabel;

    private static Dictionary<Terrain, Texture2D> TerrainTextures { get; } = new()
    {
        { Plains, Load<Texture2D>("res://textures/terrain/plains.jpg") },
        { Beach, Load<Texture2D>("res://textures/terrain/beach.jpg") },
        { Desert, Load<Texture2D>("res://textures/terrain/desert.jpg") },
        { Mountain, Load<Texture2D>("res://textures/terrain/mountain.jpg") },
        { Ice, Load<Texture2D>("res://textures/terrain/ice.jpg") },
        { Water, Load<Texture2D>("res://textures/terrain/water.jpg") },
        { Coast, Load<Texture2D>("res://textures/terrain/coast.jpg") },
        { Forest, Load<Texture2D>("res://textures/terrain/forest.jpg") }
    };

    private TextureRect TerrainImage => _terrainImage ??=
        GetNode<TextureRect>("TerrainImage") ?? throw new NullReferenceException();

    private Label TerrainLabel => _terrainLabel ??=
        GetNode<Label>("TerrainLabel") ?? throw new NullReferenceException();

    private Label FoodLabel => _foodLabel ??=
        GetNode<Label>("FoodLabel") ?? throw new NullReferenceException();

    private Label ProductionLabel => _productionLabel ??=
        GetNode<Label>("ProductionLabel") ?? throw new NullReferenceException();

    public void SetHex(Hex hex)
    {
        TerrainLabel.Text = $"Terrain: {hex.Terrain}";
        FoodLabel.Text = $"Food: {hex.Food}";
        ProductionLabel.Text = $"Production: {hex.Production}";

        TerrainImage.Texture = TerrainTextures[hex.Terrain];
    }
}