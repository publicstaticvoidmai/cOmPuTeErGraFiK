using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Board
{
    public class Board
    {
        public IReadOnlyList<LogicalPiece> LogicalState { get; private set; }
        private readonly Piece[,] _physicalState;
        
        public Board(int edgeLength)
        {
            if (edgeLength < 6 || edgeLength > 10 || edgeLength % 2 != 0) edgeLength = 8;
            
            // TODO @Mai you have to get the board to have the right edgelength somewhere hereabouts
            LogicalState = new List<LogicalPiece>(edgeLength * edgeLength).AsReadOnly();
            _physicalState = new Piece[edgeLength, edgeLength];
        }

        public Board With(LogicalPiece piece) => _With(piece, this);
        
        public Board Without(LogicalPiece piece) => _Without(piece, this);
        
        public Board With(Move move) => 
            _With(
                move.Origin, 
                move.Destination, 
                this
            );
        
        public Board With(List<Move> moves)
        {
            Board board = this;
            foreach (var move in moves)
            {
                board = _With(move.Origin, move.Destination, board); // foldleft ObjectOriented STYLE YEAH!!1!
            }

            return board;
        }
        
        // ------------------------------------ These are Companion Functions ------------------------------------ //
        
        private Board _With(LogicalPiece piece, Board board)
        {
            var physicalState = AddToGameObjectState(piece, board._physicalState);
            var logicalState = AddToLogicalState(piece, board.LogicalState);
            
            return new Board(logicalState, physicalState);
        }

        private Board _Without(LogicalPiece piece, Board board)
        {
            var logicalState = RemoveFromLogicalState(piece, board.LogicalState);
            var physicalState = RemoveFromGameObjectState(piece, board._physicalState);

            return new Board(logicalState, physicalState);
        }

        private static Board _With(LogicalPiece played, LogicalPiece bound, Board board)
        {
            int Step(int from, int to) => from + to.CompareTo(from);
            Func<PlayerColor, LogicalPiece> StepTowards(LogicalPiece origin, LogicalPiece destination) => 
                playerColor => new LogicalPiece(Step(origin.X, destination.X), Step(origin.Z, destination.Z), playerColor);

            LogicalPiece next = played;
            var currentLogicalState = board.LogicalState;
            var currentPhysicalState = board._physicalState;

            while (!next.Equals(bound))
            {
                var advanceOneStepToBoundIn = StepTowards(next, bound);

                LogicalPiece toRemove = advanceOneStepToBoundIn(played.Color.Opposing());
                LogicalPiece toAdd = advanceOneStepToBoundIn(played.Color);

                currentLogicalState = AddToLogicalState(
                    toAdd, 
                    RemoveFromLogicalState(
                        toRemove, 
                        currentLogicalState
                        )
                    );
                currentPhysicalState = AddToGameObjectState(
                    toAdd, 
                    RemoveFromGameObjectState(
                        toRemove, 
                        currentPhysicalState
                        )
                    );
                
                next = toAdd;
            }
            
            return new Board(currentLogicalState, currentPhysicalState);
        }
        
        private Board(IReadOnlyList<LogicalPiece> logicalState, Piece[,] physicalState)
        {
            LogicalState = logicalState;
            _physicalState = physicalState;
        }

        private static IReadOnlyList<LogicalPiece> AddToLogicalState(LogicalPiece piece, IReadOnlyList<LogicalPiece> state)
        {
            return new List<LogicalPiece>(state) {piece}.AsReadOnly();
        }
        
        private static IReadOnlyList<LogicalPiece> RemoveFromLogicalState(LogicalPiece piece, IReadOnlyList<LogicalPiece> state)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(state);
            intermediateLogicalState.Remove(piece);
            return intermediateLogicalState.AsReadOnly();
        }

        private static Piece[,] AddToGameObjectState(LogicalPiece piece, Piece[,] gameObjectState)
        {
            gameObjectState[piece.X, piece.Z] = InstantiateAsPiece(piece);
            return gameObjectState;
        }
        
        private static Piece[,] RemoveFromGameObjectState(LogicalPiece piece, Piece[,] gameObjectState)
        {
            MonoBehaviour.Destroy(gameObjectState[piece.X, piece.Z]);
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
                .AddComponent<Piece>();
        }
    }
}