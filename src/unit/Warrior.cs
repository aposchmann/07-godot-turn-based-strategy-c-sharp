namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Warrior : Unit
{
    public Warrior()
    {
        UnitName = "Warrior";
        ProductionRequired = 50;

        CurrentHealth = MaxHealth = 3;
        CurrentMoves = MaxMoves = 1;

        AttackValue = 2;
    }
}