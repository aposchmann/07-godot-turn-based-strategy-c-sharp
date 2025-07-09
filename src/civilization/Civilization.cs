using System;
using System.Collections.Generic;
using Godot;

namespace de.nodapo.turnbasedstrategygame.civilization;

public class Civilization
{
    public readonly List<City> Cities = [];
    public int Id;

    public required string Name;

    public bool PlayerCivilization;

    public Color TerritoryColor;

    public int TerritoryColorId;

    public void SetRandomColor()
    {
        var random = new Random();

        TerritoryColor = new Color(random.Next(255) / 255f, random.Next(255) / 255f, random.Next(255) / 255f);
    }
}