using System.Collections.Generic;
using Godot;

namespace de.nodapo.turnbasedstrategygame.civilization;

public class Civilization
{
    public int Id;

    public List<City> Cities = [];

    public Color TerritoryColor;

    public int TerritoryColorId;

    public string name;

    public bool playerCivilization;
}