using System.Collections.Generic;
using Models.Board;
using Models.Player;
using UnityEngine;

namespace Models
{
    public class Game : MonoBehaviour
    {
        private static readonly int BoardLength = PlayerPrefs.GetInt("BoardLength");
        
        public GameObject whitePref;
        public GameObject blackPref;

        private bool _isWhitesTurn;

        public readonly Piece[,] State = new Piece[BoardLength, BoardLength];
        public readonly List<Move> ValidMoves = new List<Move>();
        
        public ComputerPlayer black = new ComputerPlayer(PlayerColor.Black);
        public HumanPlayer white = new HumanPlayer(PlayerColor.White);


        public static Game Instance;
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
            var nextHumanMove = white.GetNextMove(); // nullable
            if (nextHumanMove == null) return;
            
            PlacePiece(nextHumanMove.Item1, nextHumanMove.Item2, nextHumanMove.Item3);
        }

        public PlayerColor GetCurrentColor()
        {
            return _isWhitesTurn ? PlayerColor.White : PlayerColor.Black;
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
            
            State[x, z] = piece.GetComponent<Piece>();
        }

        private void FlipPieces(int originX, int originZ, int destX, int destZ, PlayerColor color)
        {
            
        }

        private Piece GetPieceAt(int x, int z)
        {
            bool IsValid(int bound) => bound <= BoardLength - 1 && bound >= 0;

            return (IsValid(x) && IsValid(z)) ? State[x, z] : null;
        }

        private GameObject GetPrefForColor(PlayerColor color)
        {
            return color == PlayerColor.Black ? blackPref : whitePref;
        }
    }
}