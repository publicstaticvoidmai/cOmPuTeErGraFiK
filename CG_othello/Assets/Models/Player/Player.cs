using System;

namespace Models
{
    public interface Player
    {
        Tuple<int, int, PlayerColor> GetNextMove();
    }
}