using System;
using de.nodapo.turnbasedstrategygame.Map;
using Godot;

namespace de.nodapo.turnbasedstrategygame.Terrain;

public partial class TerrainPanel : Panel
{
    private Label? _foodLabel;
    private Label? _productionLabel;

    private TextureRect? _terrainImage;

    private Label? _terrainLabel;

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
    }
}