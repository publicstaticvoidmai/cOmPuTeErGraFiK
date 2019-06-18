using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Models.Board;
using Models.Player;
using UnityEngine;

namespace Models
{
    public class Game : MonoBehaviour
    {
        public int BoardLength { get; private set; }

        public GameObject whitePref;
        public GameObject blackPref;

        public IReadOnlyList<LogicalPiece> State => _board.LogicalState;

        private Board.Board _board;
        
        private ComputerPlayer _black;
        private HumanPlayer _white;
        
        public List<Move> ValidMoves { get; private set; }
        private bool _isWhitesTurn;

        public static Game Instance;
        
        public void Awake()
        {
            BoardLength = PlayerPrefs.GetInt("BoardLength");
            _board = new Board.Board(BoardLength);
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
            
            // this should not be placepiece but rather applyMove
            ApplyPieceToState(nextHumanMove.Item1, nextHumanMove.Item2, nextHumanMove.Item3);
        }

        public Player.Player GetCurrentPlayer()
        {
            return _isWhitesTurn ? (Player.Player) _white : _black;
        }

        public PlayerColor GetCurrentColor()
        {
            return _isWhitesTurn ? PlayerColor.White : PlayerColor.Black;
        }
        
        public GameObject GetPrefForColor(PlayerColor color)
        {
            return color == PlayerColor.Black ? blackPref : whitePref;
        }


        private void GenerateBoard()
        {
            int middle = BoardLength / 2;
            int offMiddle = middle - 1;
            
            ApplyPieceToState(offMiddle, offMiddle, PlayerColor.Black);
            ApplyPieceToState(offMiddle, middle, PlayerColor.White);
            ApplyPieceToState(middle, middle, PlayerColor.Black);
            ApplyPieceToState(middle, offMiddle, PlayerColor.White);
        }

        private ReadOnlyCollection<Piece> ApplyPieceToState(int x, int z, PlayerColor color, ReadOnlyCollection<Piece> state)
        {
            List<Piece> newState = new List<Piece>(state);
            GameObject pref = GetPrefForColor(color);
            GameObject piece = Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
            
            newState.Add(piece.AddComponent<Piece>().Init(x, z, color));
            return newState.AsReadOnly();
        }

        private void ApplyMove(Move move)
        {
            
        }

        private Piece GetPieceAt(int x, int z)
        {
            bool IsValid(int bound) => bound <= BoardLength - 1 && bound >= 0;

            return (IsValid(x) && IsValid(z)) ? State.Find(piece => piece.X == x && piece.Z == z) : null;
        }

        private void NextPlayer()
        {
            _isWhitesTurn = !_isWhitesTurn;
            ValidMoves = GetCurrentPlayer().GetPotentialMoves();
        }
    }
}