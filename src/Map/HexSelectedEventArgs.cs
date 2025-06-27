using System;

namespace de.nodapo.turnbasedstrategygame.Map;

public class HexSelectedEventArgs : EventArgs
{
    public required Hex Hex { get; init; }
}