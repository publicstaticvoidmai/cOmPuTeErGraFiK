using Models.Player;
using UnityEngine;

namespace Models.Board
{
    public class Piece : MonoBehaviour
    {
        public readonly int x;
        public readonly int z;
        public PlayerColor Color;
        
        public Piece(int x, int z, PlayerColor color)
        {
            this.x = x;
            this.z = z;
            Color = color;
        }
    }
}