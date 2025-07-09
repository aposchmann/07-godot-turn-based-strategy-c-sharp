using System.Collections.Generic;
using Godot;

namespace de.nodapo.turnbasedstrategygame.civilization;

public class Civilization
{
    public List<City> Cities = [];
    public int Id;

    public string name;

    public bool playerCivilization;

    public Color TerritoryColor;

    public int TerritoryColorId;
}