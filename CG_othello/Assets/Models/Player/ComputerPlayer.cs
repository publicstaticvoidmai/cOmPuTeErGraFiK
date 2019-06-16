using System;
using System.Collections.Generic;
using Models.Board;

namespace Models.Player
{
    public class ComputerPlayer : Player
    {
        private PlayerColor _color;
        public PlayerColor Color
        {
            get => _color;
        }

        public ComputerPlayer Init(PlayerColor color)
        {
            _color = color;
            return this;
        }

        private void Start()
        {
            
        }

        public override Tuple<int, int, PlayerColor> GetNextMove()
        {
            throw new NotImplementedException();
        }

        public override List<Move> GetPotentialMoves()
        {
            throw new NotImplementedException();
        }

        public override void SetNextMove(int x, int z)
        {
            throw new NotImplementedException();
        }
    }
}