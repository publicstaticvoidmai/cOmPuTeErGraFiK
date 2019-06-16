using System;
using Models.Board;
using UnityEngine;

namespace Models
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        private const int BoardLength = 8;

        private readonly Piece[,] _state = new Piece[BoardLength, BoardLength];
        private bool _isWhitesTurn;
        
        public GameObject whitePref;
        public GameObject blackPref;

        public void Awake()
        {
            Instance = this;
        }
        
        public void Start()
        {
            GenerateBoard();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }


        private void GenerateBoard()
        {
            int middle = BoardLength / 2;
            int offMiddle = middle - 1;
            
            PlacePiece(offMiddle, offMiddle, PlayerColor.Black);
            PlacePiece(offMiddle, middle, PlayerColor.White);
            PlacePiece(middle, middle, PlayerColor.Black);
            PlacePiece(middle, offMiddle, PlayerColor.White);
        }

        private void PlacePiece(int x, int z, PlayerColor color)
        {
            GameObject pref = GetPrefForColor(color);
            GameObject piece = Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
            
            _state[x, z] = piece.GetComponent<Piece>();
        }

        private void FlipPieces(int originX, int originZ, int destX, int destZ, PlayerColor color)
        {
            
        }

        private Piece GetPieceAt(int x, int z)
        {
            bool IsValid(int bound) => bound <= BoardLength - 1 && bound >= 0;

            return (IsValid(x) && IsValid(z)) ? _state[x, z] : null;
        }

        private GameObject GetPrefForColor(PlayerColor color)
        {
            return color == PlayerColor.Black ? blackPref : whitePref;
        }
    }
}