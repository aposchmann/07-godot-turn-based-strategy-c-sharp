using de.nodapo.turnbasedstrategygame.map;

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

    protected override void Move(Hex hex)
    {
        base.Move(hex);

        if (hex is { IsCityCenter: true, OwnerCity: not null } && hex.OwnerCity.Civilization != Civilization)
        {
            hex.OwnerCity.Civilization = Civilization;
        }
    }
}