using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class CityPanel : Panel
{
    private Label? _foodLabel;
    private Label? _nameLabel;
    private Label? _populationLabel;
    private Label? _productionLabel;

    private Label NameLabel => _nameLabel ??= GetNode<Label>("CityName");
    private Label PopulationLabel => _populationLabel ??= GetNode<Label>("Population");
    private Label FoodLabel => _foodLabel ??= GetNode<Label>("Food");
    private Label ProductionLabel => _productionLabel ??= GetNode<Label>("Production");

    private City? _city;

    public void SetCity(City city)
    {
        _city = city;

        Refresh();
    }

    public void Refresh()
    {
        NameLabel.Text = _city?.CityName;
        PopulationLabel.Text = $"Population: {_city?.Population}";
        FoodLabel.Text = $"Food: {_city?.TotalFood}";
        ProductionLabel.Text = $"Production: {_city?.TotalProduction}";
    }
}