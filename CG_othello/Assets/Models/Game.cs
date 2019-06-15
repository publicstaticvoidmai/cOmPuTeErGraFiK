using UnityEngine;

namespace Models
{
    public class Game : MonoBehaviour
    {
        public static Game instance;
        
        private GameObject[,] state = new GameObject[8,8];
        
        public GameObject whitePref;
        public GameObject blackPref;
        
        private void Start()
        {
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            PlacePiece(3, 3, PlayerColor.Black);
            PlacePiece(4, 4, PlayerColor.Black);
            PlacePiece(3, 4, PlayerColor.White);
            PlacePiece(4, 3, PlayerColor.White);
        }

        private void PlacePiece(int x, int z, PlayerColor color)
        {
            GameObject pref = null;
            switch (color)
            {
                case PlayerColor.White:
                    pref = whitePref;
                    break;
                case PlayerColor.Black:
                    pref = blackPref;
                    break;
            }
            GameObject piece = Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
            state[x, z] = piece;
        }

        private void FlipPieces(int originX, int originZ, int destX, int destZ, PlayerColor color)
        {
            
        }
    }
}