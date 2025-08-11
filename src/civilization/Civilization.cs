using System;
using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.city;
using de.nodapo.turnbasedstrategygame.unit;
using Godot;

namespace de.nodapo.turnbasedstrategygame.civilization;

public class Civilization
{
    private static readonly Random Random = new();

    public readonly List<City> Cities = [];
    public readonly List<Unit> Units = [];

    public required string Name;

    public required bool PlayerCivilization;

    public Color TerritoryColor;

    public int TerritoryColorId;

    public int MaximumUnits => Cities.Count * 3;

    public void SetRandomColor()
    {
        TerritoryColor = new Color(Random.NextSingle(), Random.NextSingle(), Random.NextSingle());
    }

    public void ProcessEndTurn()
    {
        Cities.ForEach(city => city.ProcessEndTurn());
        Units.ForEach(unit => unit.ProcessEndTurn());

        if (PlayerCivilization) return;

        Cities.ForEach(city =>
        {
            var unitQueueChance = Random.Next(30);

            if (unitQueueChance > 27)
            {
                city.AddUnitToBuildQueue(new Warrior());
            }

            if (unitQueueChance > 28)
            {
                city.AddUnitToBuildQueue(new Settler());
            }
        });

        for (var i = Units.Count - 1; i >= 0; i--)
        {
            var unit = Units[i];

            unit.RandomMove();

            if (unit is Settler settler && Random.Next(10) > 8)
            {
                settler.FoundCity();
            }
        }
    }
}