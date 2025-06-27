using System.Collections.Generic;
using de.nodapo.turnbasedstrategygame.Map;
using Godot;

namespace de.nodapo.turnbasedstrategygame;

public partial class City : Node2D
{
    private HexMap? _hexMap;

    private HexMap HexMap => _hexMap ??= GetNode<HexMap>("/root/Game/HexMap");

    public Vector2I centerCoordinates;

    public List<Hex> territory;

    public List<Hex> borderTilePool;

    public string name;

    private Label _nameLabel;

    private Sprite2D _imageSprite;
}