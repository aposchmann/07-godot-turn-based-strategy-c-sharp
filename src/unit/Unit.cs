using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class Unit : Node2D
{
    public int ProductionRequired { get; protected set; }
    public string UnitName { get; protected set; } = null!;
}