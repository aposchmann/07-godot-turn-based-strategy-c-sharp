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

    public int MaximumUnits => Cities.Count * 3;

    public void SetRandomColor()
    {
        var random = new Random();

        TerritoryColor = new Color(random.NextSingle(), random.NextSingle(), random.NextSingle());
    }

    public void ProcessEndTurn()
    {
        Cities.ForEach(city => city.ProcessEndTurn());
        Units.ForEach(unit => unit.ProcessEndTurn());

        if (PlayerCivilization) return;

        var random = new Random();

        Cities.ForEach(city =>
        {
            var unitQueueChance = random.Next(30);

            if (unitQueueChance > 27)
            {
                city.AddUnitToBuildQueue(new Warrior());
            }

            if (unitQueueChance > 28)
            {
                city.AddUnitToBuildQueue(new Settler());
            }
        });

        foreach (var unit in Units)
        {
            unit.RandomMove();

            if (unit is Settler settler && random.Next(10) > 8)
            {
                settler.FoundCity();
            }
        }
    }
}