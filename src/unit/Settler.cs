namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Settler : Unit
{
    public Settler()
    {
        UnitName = "Settler";
        ProductionRequired = 100;

        currentHealth = maxHealth = 1;
        currentMoves = maxMoves = 2;
    }
}