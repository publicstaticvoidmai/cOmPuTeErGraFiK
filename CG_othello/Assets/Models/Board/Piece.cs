using Models.Player;
using UnityEngine;

namespace Models.Board
{
    public class Piece : MonoBehaviour
    {
        private LogicalPiece _underlying;
        public int X => _underlying.X;
        public int Z => _underlying.Z;
        public PlayerColor Color => _underlying.Color;
        
        public Piece Init(LogicalPiece piece)
        {
            _underlying = piece;
            return this;
        }

        public LogicalPiece ToLogicalPiece()
        {
            return _underlying;
        }
    }
}