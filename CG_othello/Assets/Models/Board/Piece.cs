using Models.Player;
using UnityEngine;

namespace Models.Board
{
    public class Piece : MonoBehaviour
    {
        private int _x;
        public int X => _x;
        
        private int _z;
        public int Z => _z;
        
        public PlayerColor Color;
        
        public Piece Init(int x, int z, PlayerColor color)
        {
            _x = x;
            _z = z;
            Color = color;
            
            return this;
        }

        public override bool Equals(object other)
        {
            Piece otherPiece = other as Piece;
            
            return otherPiece 
                   != null && 
                   otherPiece._x == _x && 
                   otherPiece._z == _z && 
                   otherPiece.Color == Color;
        }
    }
}