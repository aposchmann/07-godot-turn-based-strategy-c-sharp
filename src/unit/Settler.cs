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

    public override void _Ready()
    {
        base._Ready();
    }
}