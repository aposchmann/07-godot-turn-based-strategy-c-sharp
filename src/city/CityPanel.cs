using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class CityPanel : Panel
{
    private Label? _nameLabel;
    private Label? _populationLabel;
    private Label? _foodLabel;
    private Label? _productionLabel;

    private Label NameLabel => _nameLabel ??= GetNode<Label>("CityName");
    private Label PopulationLabel => _populationLabel ??= GetNode<Label>("Population");
    private Label FoodLabel => _foodLabel ??= GetNode<Label>("Food");
    private Label ProductionLabel => _productionLabel ??= GetNode<Label>("Production");

    public void SetCity(City city)
    {
        NameLabel.Text = city.CityName;
        PopulationLabel.Text = $"Population: {city.Population}";
        FoodLabel.Text = $"Food: {city.TotalFood}";
        ProductionLabel.Text = $"Production: {city.TotalProduction}";
    }
}