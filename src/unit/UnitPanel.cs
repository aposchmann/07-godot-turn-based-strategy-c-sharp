using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class UnitPanel : Panel
{
    private Label? _healthLabel;
    private Label? _movesLabel;
    private Label? _typeLabel;
    private Unit? _unit;
    private TextureRect? _unitImage;

    private TextureRect UnitImage => _unitImage ??= GetNode<TextureRect>("UnitImage");
    private Label TypeLabel => _typeLabel ??= GetNode<Label>("UnitType");
    private Label HealthLabel => _healthLabel ??= GetNode<Label>("UnitHealth");
    private Label MovesLabel => _movesLabel ??= GetNode<Label>("UnitMoves");

    public void setUnit(Unit unit)
    {
        _unit = unit;

        Refresh();
    }

    public void Refresh()
    {
    }
}