using System;

namespace de.nodapo.turnbasedstrategygame.map;

public class HexSelectedEventArgs : EventArgs
{
    public required Hex Hex { get; init; }
}