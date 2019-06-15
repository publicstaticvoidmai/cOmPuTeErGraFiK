using Models.Player;
using UnityEngine;

namespace Models.Board
{
    public class Board : MonoBehaviour
    {
        private Piece[,] state = new Piece[8,8];
        
        public GameObject whitePref;
        public GameObject blackPref;
        
        private void Start()
        {
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            placePiece(3, 3, PlayerColor.Black);
            placePiece(4, 4, PlayerColor.Black);
            placePiece(3, 4, PlayerColor.White);
            placePiece(4, 3, PlayerColor.White);
        }

        private void placePiece(int x, int z, PlayerColor color)
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
            Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
        }

        private void OnMouseOver()
        {
            //If your mouse hovers over the GameObject with the script attached, output this message
            Debug.Log("Mouse is over GameObject.");
        }

        private void OnMouseExit()
        {
            //The mouse is no longer hovering over the GameObject so output this message each frame
            Debug.Log("Mouse is no longer on GameObject.");
        }
    }
}