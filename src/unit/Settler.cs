namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Settler : Unit
{
    public Settler()
    {
        UnitName = "Settler";
        ProductionRequired = 100;

        CurrentHealth = MaxHealth = 1;
        CurrentMoves = MaxMoves = 2;
    }
}