namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Settler : Unit
{
    public Settler()
    {
        UnitName = "Settler";
        ProductionRequired = 100;

        CurrentHealth = MaxHealth = 1;
        CurrentMoves = MaxMoves = 2;

        AttackValue = 0;
    }

    public void FoundCity()
    {
        if (Civilization is null) return;
        if (HexMap.GetHex(Coordinates).OwnerCity is not null) return;

        foreach (var hex in HexMap.GetSurroundingHexes(Coordinates))
            if (hex.OwnerCity is not null)
                return;

        HexMap.CreateCity(Civilization, Coordinates, $"City {Coordinates}");

        DestroyUnit();
    }
}