using Godot;

namespace de.nodapo.turnbasedstrategygame.unit;

public partial class UnitPanel : Panel
{
    private VBoxContainer? _actionList;
    private Label? _healthLabel;
    private Label? _movesLabel;
    private Label? _typeLabel;
    private Unit? _unit;
    private TextureRect? _unitImage;

    private TextureRect UnitImage => _unitImage ??= GetNode<TextureRect>("UnitImage");
    private Label TypeLabel => _typeLabel ??= GetNode<Label>("UnitType");
    private Label HealthLabel => _healthLabel ??= GetNode<Label>("UnitHealth");
    private Label MovesLabel => _movesLabel ??= GetNode<Label>("UnitMoves");
    private VBoxContainer ActionList => _actionList ??= GetNode<VBoxContainer>("UnitActionList");

    public void SetUnit(Unit unit)
    {
        _unit = unit;

        if (unit.GetType() == typeof(Settler))
        {
            var foundCityButton = new Button
            {
                Text = "Found City"
            };

            ActionList.AddChild(foundCityButton);

            foundCityButton.Pressed += ((Settler)unit).FoundCity;
        }

        Refresh();
    }

    public void Refresh()
    {
        if (_unit == null) return;

        UnitImage.Texture = Unit.UnitTextures[_unit.GetType()];
        TypeLabel.Text = $"Unit Type: {_unit.UnitName}";
        HealthLabel.Text = $"Health: {_unit.CurrentHealth}/{_unit.MaxHealth}";
        MovesLabel.Text = $"Moves: {_unit.CurrentMoves}/{_unit.MaxMoves}";
    }
}