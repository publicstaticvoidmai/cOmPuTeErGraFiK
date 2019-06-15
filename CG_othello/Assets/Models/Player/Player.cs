using System;

namespace Models.Player
{
    public interface Player
    {
        Tuple<int, int, PlayerColor> GetNextMove();
    }
}