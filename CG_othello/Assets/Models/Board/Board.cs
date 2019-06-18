using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Models.Board
{
    public class Board
    {
        public ReadOnlyCollection<LogicalPiece> LogicalState { get; private set; }
        private readonly Piece[,] _gameObjectState;
        
        public Board(int edgeLength)
        {
            if (edgeLength < 6 || edgeLength > 10 || edgeLength % 2 != 0) edgeLength = 8;
            
            // TODO @Mai you have to get the board to have the right edgelength somewhere hereabouts
            LogicalState = new List<LogicalPiece>(edgeLength * edgeLength).AsReadOnly();
            _gameObjectState = new Piece[edgeLength, edgeLength];
        }
        
        public Board With(LogicalPiece piece)
        {
            return _With(piece, this);
        }

        private Board _With(LogicalPiece piece, Board board)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(board.LogicalState);
            
            Piece visiblePiece = 
            
            intermediateLogicalState.Add(piece);
            AddToGameObjectState(visiblePiece);
            
            board.LogicalState = intermediateLogicalState.AsReadOnly();
            return this;
        }

        public Board Without(LogicalPiece piece)
        {
            return _Without(piece, this);
        }

        private Board _Without(LogicalPiece piece, Board board)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(board.LogicalState);
            intermediateLogicalState.Remove(piece);
            MonoBehaviour.Destroy(_gameObjectState[piece.X, piece.Z]);
            
            _gameObjectState[piece.X, piece.Z] = null;
            board.LogicalState = intermediateLogicalState.AsReadOnly();

            return this;
        }

        public Board With(Move move)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(LogicalState);
            return _With(move.Origin, move.Destination, intermediateLogicalState, this);
        }

        private static Board _With(LogicalPiece played, LogicalPiece bound, List<LogicalPiece> state, Board board)
        {
            if (played.Equals(bound))
            {
                board.LogicalState.Intersect(state.AsReadOnly())
                board.LogicalState = state.AsReadOnly();
                return board;
            }
            
            int Step(int from, int to) => from + to.CompareTo(from);
            Func<PlayerColor, LogicalPiece> StepTowards(LogicalPiece o, LogicalPiece d) => 
                t => new LogicalPiece(Step(o.X, d.X), Step(o.Z, d.Z), t);
            Func<PlayerColor, LogicalPiece> field = StepTowards(played, bound);

            LogicalPiece toRemove = field(played.Color.Opposing());
            LogicalPiece toAdd = field(played.Color);

            state.Remove(toRemove);
            if (!state.Contains(toAdd)) state.Add(toAdd);

            return _With(toAdd, bound, state, board);
        }

        private static Piece[,] AddToGameObjectState(LogicalPiece piece, Piece[,] gameObjectState)
        {
            gameObjectState[piece.X, piece.Z] = InstantiateAsPiece(piece);
            return gameObjectState;
        }
        
        private static Piece[,] RemoveFromGameObjectState(LogicalPiece piece, Piece[,] gameObjectState)
        {
            gameObjectState[piece.X, piece.Z] = null;
            return gameObjectState;
        }

        private static Piece InstantiateAsPiece(LogicalPiece piece)
        {
            // this has the very intended sideeffect of actually creating the Piece in the GameWorld.
            // Should be I/O but it isn't
            return MonoBehaviour
                .Instantiate(
                    Game.Instance.GetPrefForColor(piece.Color), 
                    new Vector3(piece.X, 0f, piece.Z), 
                    Quaternion.identity
                    )
                .AddComponent<Piece>()
                .Init(piece);
        }
    }
}