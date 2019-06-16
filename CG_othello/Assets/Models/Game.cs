using System;
using System.Collections.Generic;
using Models.Board;
using Models.Player;
using UnityEngine;

namespace Models
{
    public class Game : MonoBehaviour
    {
        private int _boardLength;
        public int BoardLength => _boardLength;

        public GameObject whitePref;
        public GameObject blackPref;

        private bool _isWhitesTurn;

        private List<Piece> _state;
        public List<Piece> State => _state;

        private List<Move> _validMoves;
        public List<Move> ValidMoves => _validMoves;
        
        private ComputerPlayer _black;
        private HumanPlayer _white;
        
        public static Game Instance;
        public void Awake()
        {
            _boardLength = PlayerPrefs.GetInt("BoardLength");
            if (_boardLength < 6 || _boardLength > 10 || _boardLength % 2 != 0)
            {
                _boardLength = 8;
            }
            
            _state = new List<Piece>(64);
            _validMoves = new List<Move>();
            _black = gameObject.AddComponent<ComputerPlayer>().Init(PlayerColor.Black);
            _white = gameObject.AddComponent<HumanPlayer>().Init(PlayerColor.White);
            _isWhitesTurn = false;
            Instance = this;
        }
        
        public void Start()
        {
            GenerateBoard();
            NextPlayer();
        }

        public void Update()
        {
            var nextHumanMove = _white.GetNextMove(); // nullable
            if (nextHumanMove == null) return;
            
            PlacePiece(nextHumanMove.Item1, nextHumanMove.Item2, nextHumanMove.Item3);
        }

        public Player.Player GetCurrentPlayer()
        {
            return _isWhitesTurn ? (Player.Player) _white : _black;
        }

        public PlayerColor GetCurrentColor()
        {
            return _isWhitesTurn ? PlayerColor.White : PlayerColor.Black;
        }


        private void GenerateBoard()
        {
            int middle = _boardLength / 2;
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
            
            _state.Add(piece.AddComponent<Piece>().Init(x, z, color));
        }

        private void FlipPieces(int originX, int originZ, int destX, int destZ, PlayerColor color)
        {
            throw new NotImplementedException();
        }

        private Piece GetPieceAt(int x, int z)
        {
            bool IsValid(int bound) => bound <= _boardLength - 1 && bound >= 0;

            return (IsValid(x) && IsValid(z)) ? _state.Find(piece => piece.X == x && piece.Z == z) : null;
        }

        private GameObject GetPrefForColor(PlayerColor color)
        {
            return color == PlayerColor.Black ? blackPref : whitePref;
        }

        private void NextPlayer()
        {
            _isWhitesTurn = !_isWhitesTurn;
            var moves = GetCurrentPlayer().GetPotentialMoves();
            Debug.Log("Found " + moves.Count + " moves");
            _validMoves = moves;
        }
    }
}