using System.Collections.Generic;
using System.Linq;
using Models.Board;
using Models.Player;
using UnityEngine;
using UnityEngine.UI;

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

        public Text playerText;
        public Text scoreText;
        public GameObject canvas;
        public Camera mainCamera;

        public IPlayer CurrentPlayer { get; private set; }
        public static Game Instance;

        private bool _isWhitesTurn;
        
        public void Awake()
        {
            BoardLength = PlayerPrefs.GetInt("BoardLength");
            if (BoardLength < 6 || BoardLength > 10 || BoardLength % 2 != 0) BoardLength = 8;

            
            mainCamera.transform.position = GetCameraTransform();
            
            _logicalBoard = new Board.Board(BoardLength);

            _player1 = GetPlayerFor(PlayerColor.Black);
            _player2 = GetPlayerFor(PlayerColor.White);

            CurrentPlayer = _player1;
            Instance = this;
        }

        private Vector3 GetCameraTransform()
        {
            Vector3 cameraTransform = new Vector3(2.5f, 7.0f, 1.0f);
            Vector3 offset = new Vector3(1.0f, 1.0f, 0.75f);
            int numberOfAdditions = (BoardLength / 2) - 3;
            while (numberOfAdditions != 0)
            {
                cameraTransform += offset;
                numberOfAdditions--;
            }

            return cameraTransform;
        }

        private IPlayer GetPlayerFor(PlayerColor color)
        {
            if (PlayerPrefs.GetString(color.ToString()) == "AI")
            {
                int difficulty = PlayerPrefs.GetInt(color + "_AI", 2);
                return ComputerPlayer.Create(color, difficulty);
            }
            return HumanPlayer.Create(color);
        }
        
        public void Start()
        {
            GeneratePhysicalBoard();
            NextPlayer(false); // human will start
        }

        public void Update()
        {
            if (CurrentPlayer.HasPassed() && _player2.HasPassed())
            {
                int ScoreFor(PlayerColor color) => _logicalBoard.LogicalState.Count(piece => piece.Color == color);
                int scoreBlack = ScoreFor(PlayerColor.Black);
                int scoreWhite = ScoreFor(PlayerColor.White);

                if (scoreWhite == scoreBlack)
                {
                    playerText.text = "Draw!";
                    scoreText.text = scoreBlack + " : " + scoreWhite;
                }
                else if(scoreBlack > scoreWhite)
                {
                    playerText.text = "BLACK wins!";
                    scoreText.text = scoreBlack + " : " + scoreWhite;
                }
                else
                {
                    playerText.text = "WHITE wins!";
                    scoreText.text = scoreWhite + " : " + scoreBlack;
                }
                
                canvas.SetActive(true);
                return;
            }
            if (!CurrentPlayer.HasNextMove())
            {
                NextPlayer(true);
                return;
            }

            List<Move> nextMove = CurrentPlayer.GetNextMove();
            if (nextMove.Count == 0) return;
            
            _logicalBoard = _logicalBoard.With(nextMove);

            NextPlayer(false);
        }
        
        public GameObject GetPrefForColor(PlayerColor color) => color == PlayerColor.Black ? blackPref : whitePref;
        
        private void GeneratePhysicalBoard()
        {
            int middle = BoardLength / 2;
            int offMiddle = middle - 1;
            
            void CreateTileAt(int x, int z) => 
                Instantiate(tilePref, new Vector3(x, 0, z), Quaternion.identity)
                    .transform.SetParent(board.transform);
            
            for (int x = 0; x < BoardLength; x++)
            {
                for (int z = 0; z < BoardLength; z++)
                {
                    CreateTileAt(x, z);
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
            if (currentHasPassed)
            {
                _player1 = CurrentPlayer.WithPass();
                CurrentPlayer = _player2;
                _player2 = _player1;
            }
            else
            {
                _player1 = CurrentPlayer.WithCalculatedPotentialMovesFrom(_logicalBoard.LogicalState);
                CurrentPlayer = _player2.WithCalculatedPotentialMovesFrom(_logicalBoard.LogicalState);
                _player2 = _player1;
            }
        }
    }
}