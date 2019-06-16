using System;
using System.Collections.Generic;
using Models.Board;

namespace Models.Player
{
    public class ComputerPlayer : Player
    {
        public readonly PlayerColor Color;

        public ComputerPlayer(PlayerColor color)
        {
            Color = color;
        }

        public override Tuple<int, int, PlayerColor> GetNextMove()
        {
            return new Tuple<int, int, PlayerColor>(1, 2, PlayerColor.Black);
        }

        public override List<Move> GetPotentialMoves()
        {
            return new List<Move>();
        }
    }
}