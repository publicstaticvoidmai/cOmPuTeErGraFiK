using System;
using System.Collections.Generic;
using Models.Board;

namespace Models.Player
{
    public class HumanPlayer : Player
    {
        private int? _nextX;
        private int? _nextZ;

        public readonly PlayerColor Color;

        public HumanPlayer(PlayerColor color)
        {
            Color = color;
        }

        public override Tuple<int, int, PlayerColor> GetNextMove()
        {
            if (!_nextX.HasValue || !_nextZ.HasValue) return null;
            
            var result = Tuple.Create(_nextX.Value, _nextZ.Value, Color);
            _nextX = null;
            _nextZ = null;

            return result;
        }

        public override List<Move> GetPotentialMoves()
        {
            return CalculatePotentialMoves(Game.Instance.State)
                .FindAll(move => move.Origin.Color.Equals(Color));
        }

        public void SetNextMove(int x, int z)
        {
            _nextX = x;
            _nextZ = z;
        }

    }
}