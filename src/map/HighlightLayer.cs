using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.city;
using Godot;

namespace de.nodapo.turnbasedstrategygame.map;

public partial class HighlightLayer : TileMapLayer
{
    private int width;
    private int height;

    private List<Hex> highlightedHexes = [];

    private City? highlightedCity;

    public void setUp(int width, int height)
    {
        this.width = width;
        this.height = height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                SetCell(new Vector2I(x, y), 0, new Vector2I(0, 3));
            }
        }

        Visible = false;
    }

    public void Highlight(City city)
    {
        ResetHighlight();

        highlightedCity = city;

        highlightedCity.Territory.ForEach(hex =>
        {
            highlightedHexes.Add(hex);

            SetCell(hex.Coordinates, -1, new Vector2I(0, 3));
        });
        
        Visible = true;
    }

    public void ResetHighlight()
    {
        foreach (var highlightedHex in highlightedHexes)
        {
            SetCell(highlightedHex.Coordinates, 0, new Vector2I(0, 3));
        }

        highlightedHexes = [];
        highlightedCity = null;
        Visible = false;
    }
}