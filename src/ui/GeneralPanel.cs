using Godot;

namespace de.nodapo.turnbasedstrategygame.ui;

public partial class GeneralPanel : Panel
{
    private int Turns;

    private Label? _turnLabel;

    private Label TurnLabel => _turnLabel ??= GetNode<Label>("TurnLabel");

    public void IncrementTurns()
    {
        Turns++;
        TurnLabel.Text = $"Turn: {Turns}";
    }
}