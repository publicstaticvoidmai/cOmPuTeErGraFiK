using System;
using System.Collections.Generic;
using Models.Board;

namespace Models.Player
{
    public class HumanPlayer : Player
    {
        private int? _nextX;
        private int? _nextZ;

        private PlayerColor _color;
        public PlayerColor Color => _color;
        
        public HumanPlayer Init(PlayerColor color)
        {
            _color = color;
            return this;
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
            var logicalPieces = Game.Instance.State;
            var k = CalculatePotentialMoves(logicalPieces);
                
            var x = k.FindAll(move => move.Origin.Color.Equals(Color));

            return x;
        }

        public override void SetNextMove(int x, int z)
        {
            _nextX = x;
            _nextZ = z;
        }

    }
}