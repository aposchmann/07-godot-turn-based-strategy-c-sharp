using System;

namespace de.nodapo.turnbasedstrategygame.map;

public class HexRightClickedEventArgs : EventArgs
{
    public required Hex Hex { get; init; }
}