using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.city;
using de.nodapo.turnbasedstrategygame.unit;
using Godot;

namespace de.nodapo.turnbasedstrategygame.civilization;

public class Civilization
{
    public readonly List<City> Cities = [];
    public readonly List<Unit> Units = [];
    public required int Id;

    public required string Name;

    public required bool PlayerCivilization;

    public Color TerritoryColor;

    public int TerritoryColorId;

    public void SetRandomColor()
    {
        var random = new Random();

        TerritoryColor = new Color(random.NextSingle(), random.NextSingle(), random.NextSingle());
    }
}