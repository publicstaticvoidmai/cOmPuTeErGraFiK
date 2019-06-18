using System.Collections.Generic;
using System.Threading.Tasks;
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

        public GameObject board;
        public GameObject tilePref;

        private Board.Board _logicalBoard; // this is the actual state
        
        private IPlayer _player1;
        private IPlayer _player2;

        public IPlayer CurrentPlayer { get; private set; }
        
        
        private bool _isWhitesTurn;

        public static Game Instance;
        
        public void Awake()
        {
            BoardLength = PlayerPrefs.GetInt("BoardLength");
            if (BoardLength < 6 || BoardLength > 10 || BoardLength % 2 != 0) BoardLength = 8;
            
            _logicalBoard = new Board.Board(BoardLength);
            _player1 = ComputerPlayer.Create(PlayerColor.Black);
            _player2 = HumanPlayer.Create(PlayerColor.White);

            CurrentPlayer = _player1;
            
            Instance = this;
        }
        
        public void Start()
        {
            GenerateBoard();
            NextPlayer(false); // human will start
        }

        public void Update()
        {
            if (!(CurrentPlayer.HasPassed() && _player2.HasPassed())) {
                if (CurrentPlayer.HasNextMove())
                {
                    List<Move> nextMove = CurrentPlayer.GetNextMove();
                    if (nextMove.Count == 0) return;

                    _logicalBoard = _logicalBoard.With(nextMove);
                    
                    NextPlayer(false);
                } else NextPlayer(true);
            }
            // wenn wir hier ankommen ist das Spiel vorbei
        }
        
        public GameObject GetPrefForColor(PlayerColor color) => color == PlayerColor.Black ? blackPref : whitePref;
        
        private void GenerateBoard()
        {
            int middle = BoardLength / 2;
            int offMiddle = middle - 1;
            
            void CreateTile(int x, int z) => 
                Instantiate(tilePref, new Vector3(x, 0, z), Quaternion.identity)
                .transform.SetParent(board.transform);
            
            for (int x = 0; x < BoardLength; x++)
            {
                for (int z = 0; z < BoardLength; z++)
                {
                    CreateTile(x, z);
                }
            }

            _logicalBoard = _logicalBoard
                .With(new LogicalPiece(offMiddle, offMiddle, PlayerColor.Black))
                .With(new LogicalPiece(offMiddle, middle, PlayerColor.White))
                .With(new LogicalPiece(middle, middle, PlayerColor.Black))
                .With(new LogicalPiece(middle, offMiddle, PlayerColor.White));
        }
        
        private void NextPlayer(bool currentHasPassed)
        {
            _player1 = currentHasPassed ? // current player gets lined up to be player 2 by becoming player one
                CurrentPlayer.WithPass() : 
                CurrentPlayer.WithCalculatedPotentialMovesFrom(_logicalBoard.LogicalState);
            
            CurrentPlayer = currentHasPassed ? // actual player 2 is being made the current player
                _player2 : 
                _player2.WithCalculatedPotentialMovesFrom(_logicalBoard.LogicalState);
            
            _player2 = _player1; // player 1 is now player 2
        }
    }
}