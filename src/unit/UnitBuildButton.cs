using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class UnitBuildButton : Button
{
    [Signal]
    public delegate void BuildUnitEventHandler(Unit unit);

    public required Unit Unit { get; set; }

    public override void _Ready()
    {
        Pressed += () => EmitSignal(SignalName.BuildUnit, Unit);
    }
}