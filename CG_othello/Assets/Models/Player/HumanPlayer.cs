using System;
using System.Collections.Generic;
using Models.Board;
using UnityEngine;

namespace Models.Player
{
  /*  public class HumanPlayer : Player
    {
        private int? _nextX;
        private int? _nextZ;

        private PlayerColor _color;

        public PlayerColor Color { get => _color; }

        private Piece Piece1;

        public HumanPlayer Init(PlayerColor color)
        {
            _color = color;
            return this;
        }

        private void Start()
        {
            Piece1 = gameObject.AddComponent<Piece>();
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

        public override void SetNextMove(int x, int z)
        {
            _nextX = x;
            _nextZ = z;
        }

    }*/
}