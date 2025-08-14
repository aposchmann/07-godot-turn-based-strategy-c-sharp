using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.city;
using Godot;

namespace de.nodapo.turnbasedstrategygame.map;

public partial class HighlightLayer : TileMapLayer
{
    private int _width;
    private int _height;

    private List<Hex> _highlightedHexes = [];

    private City? _highlightedCity;

    public void SetUp(int width, int height)
    {
        _width = width;
        _height = height;

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

        _highlightedCity = city;

        _highlightedCity.Territory.ForEach(hex =>
        {
            _highlightedHexes.Add(hex);

            SetCell(hex.Coordinates, -1, new Vector2I(0, 3));
        });

        Visible = true;
    }

    public void ResetHighlight()
    {
        foreach (var highlightedHex in _highlightedHexes)
        {
            SetCell(highlightedHex.Coordinates, 0, new Vector2I(0, 3));
        }

        _highlightedHexes = [];
        _highlightedCity = null;
        Visible = false;
    }

    public void Refresh()
    {
        if (_highlightedCity is not null)
        {
            Highlight(_highlightedCity);
        }
        else
        {
            ResetHighlight();
        }
    }
}