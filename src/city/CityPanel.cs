using Godot;

namespace de.nodapo.turnbasedstrategygame.city;

public partial class CityPanel : Panel
{
    private Label? _nameLabel;
    private Label? _populationLabel;
    private Label? _foodLabel;
    private Label? _productionLabel;

    private Label NameLabel => _nameLabel ??= GetNode<Label>("NameLabel");
    private Label PopulationLabel => _populationLabel ??= GetNode<Label>("PopulationLabel");
    private Label FoodLabel => _foodLabel ??= GetNode<Label>("FoodLabel");
    private Label ProductionLabel => _productionLabel ??= GetNode<Label>("ProductionLabel");

    public void SetCity(City city)
    {
        NameLabel.Text = city.CityName;
        PopulationLabel.Text = $"Population: {city.Population}";
        FoodLabel.Text = $"Food: {city.TotalFood}";
        ProductionLabel.Text = $"Production: {city.TotalProduction}";
    }
}