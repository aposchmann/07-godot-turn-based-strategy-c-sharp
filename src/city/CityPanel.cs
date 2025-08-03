using de.nodapo.turnbasedstrategygame.unit;
using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class CityPanel : Panel
{
    private City? _city;
    private Label? _foodLabel;
    private Label? _nameLabel;
    private Label? _populationLabel;
    private Label? _productionLabel;
    private VBoxContainer? _queue;
    private UnitBuildButton? _settlerBuildButton;
    private UnitBuildButton? _warriorBuildButton;

    private Label NameLabel => _nameLabel ??= GetNode<Label>("CityName");
    private Label PopulationLabel => _populationLabel ??= GetNode<Label>("Population");
    private Label FoodLabel => _foodLabel ??= GetNode<Label>("Food");
    private Label ProductionLabel => _productionLabel ??= GetNode<Label>("Production");

    private UnitBuildButton SettlerBuildButton => _settlerBuildButton ??=
        GetNode<UnitBuildButton>("BuildButtons/VBoxContainer/Settler");

    private UnitBuildButton WarriorBuildButton => _warriorBuildButton ??=
        GetNode<UnitBuildButton>("BuildButtons/VBoxContainer/Warrior");

    private VBoxContainer Queue => _queue ??= GetNode<VBoxContainer>("QueueItems/VBoxContainer");

    public void SetCity(City city)
    {
        _city = city;

        Refresh();

        ConnectUnitBuildSignals(city);
    }

    public void Refresh()
    {
        NameLabel.Text = _city?.CityName;
        PopulationLabel.Text = $"Population: {_city?.Population}";
        FoodLabel.Text = $"Food: {_city?.TotalFood}";
        ProductionLabel.Text = $"Production: {_city?.TotalProduction}";

        PopulateUnitQueueUi();
    }

    private void ConnectUnitBuildSignals(City city)
    {
        SettlerBuildButton.Unit = new Settler();
        WarriorBuildButton.Unit = new Warrior();

        SettlerBuildButton.BuildUnit += city.AddUnitToBuildQueue;
        SettlerBuildButton.BuildUnit += _ => Refresh();

        WarriorBuildButton.BuildUnit += city.AddUnitToBuildQueue;
        WarriorBuildButton.BuildUnit += _ => Refresh();
    }

    private void PopulateUnitQueueUi()
    {
        foreach (var queueItem in Queue.GetChildren())
        {
            Queue.RemoveChild(queueItem);
            queueItem.QueueFree();
        }

        for (var i = 0; i < _city?.UnitBuildQueue.Count; i++)
        {
            var unit = _city.UnitBuildQueue[i];

            if (i == 0)
                Queue.AddChild(new Label
                {
                    Text = $"{unit.UnitName} {_city?.UnitBuildTracker}/{unit.ProductionRequired}"
                });
            else
                Queue.AddChild(new Label
                {
                    Text = $"{unit.UnitName} 0/{unit.ProductionRequired}"
                });
        }
    }
}