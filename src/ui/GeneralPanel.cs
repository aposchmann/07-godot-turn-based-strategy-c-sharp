using Godot;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class GeneralPanel : Panel
{
    [Signal]
    public delegate void EndTurnEventHandler();

    private Button? _turnButton;

    private Label? _turnLabel;

    private int _turns;

    private Label TurnLabel => _turnLabel ??= GetNode<Label>("TurnLabel");
    private Button TurnButton => _turnButton ??= GetNode<Button>("TurnButton");

    public override void _Ready()
    {
        IncrementTurn();

        TurnButton.Pressed += () => EmitSignal(SignalName.EndTurn);
    }

    public void IncrementTurn()
    {
        _turns++;
        TurnLabel.Text = $"Turn: {_turns}";
    }
}