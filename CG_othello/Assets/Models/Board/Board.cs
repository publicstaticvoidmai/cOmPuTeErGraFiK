using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Models.Board
{
    public class Board
    {
        public IReadOnlyList<LogicalPiece> LogicalState { get; private set; }
        private readonly IReadOnlyList<Piece> _physicalState;
        
        public Board(int edgeLength)
        {
            LogicalState = new List<LogicalPiece>(edgeLength * edgeLength).AsReadOnly();
            _physicalState = new List<Piece>(edgeLength * edgeLength).AsReadOnly();
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
                board = _With(move.Origin, board);
                board = _With(move.Origin, move.Destination, board); // foldleft ObjectOriented STYLE YEAH!!1!
            }

            return board;
        }
        
        // ------------------------------------ These are Companion Functions ------------------------------------ //
        
        private Board _With(LogicalPiece piece, Board board)
        {
            if (board.LogicalState.Contains(piece)) return board;
            
            var logicalState = AddToLogicalState(piece, board.LogicalState);
            var physicalState = AddToGameObjectState(piece, board._physicalState);
            
            return new Board(logicalState, physicalState);
        }

        private Board _Without(LogicalPiece piece, Board board)
        {
            if (!board.LogicalState.Contains(piece)) return board;
            
            var logicalState = RemoveFromLogicalState(piece, board.LogicalState);
            var physicalState = RemoveFromGameObjectState(piece, board._physicalState);

            return new Board(logicalState, physicalState);
        }

        private static Board _With(LogicalPiece played, LogicalPiece bound, Board board)
        {
            // this looks like shit I'm sorry: This advances from one step in the direction of to
            int Step(int from, int to) => from + (from < to ? 1 : to == from ? 0 : -1);
            Func<PlayerColor, LogicalPiece> StepTowards(LogicalPiece origin, LogicalPiece destination) => 
                playerColor => new LogicalPiece(Step(origin.X, destination.X), Step(origin.Z, destination.Z), playerColor);

            LogicalPiece next = played;
            var currentBoard = board;

            while (!next.Equals(bound))
            {
                var advanceOneStepToBoundIn = StepTowards(next, bound);

                LogicalPiece toRemove = advanceOneStepToBoundIn(played.Color.Opposing());
                LogicalPiece toAdd = advanceOneStepToBoundIn(played.Color);

                //if (toAdd.Equals(bound)) break;
                
                currentBoard = currentBoard
                    .Without(toRemove)
                    .With(toAdd);

                next = toAdd;
            }
            
            return currentBoard;
        }
        
        private Board(IReadOnlyList<LogicalPiece> logicalState, IReadOnlyList<Piece> physicalState)
        {
            LogicalState = logicalState;
            _physicalState = physicalState;
        }

        private static IReadOnlyList<LogicalPiece> AddToLogicalState(LogicalPiece piece, IReadOnlyList<LogicalPiece> state)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(state);
            intermediateLogicalState.Add(piece);
            
            return intermediateLogicalState.AsReadOnly();
        }
        
        private static IReadOnlyList<LogicalPiece> RemoveFromLogicalState(LogicalPiece piece, IReadOnlyList<LogicalPiece> state)
        {
            List<LogicalPiece> intermediateLogicalState = new List<LogicalPiece>(state);
            intermediateLogicalState.Remove(piece);
            
            return intermediateLogicalState.AsReadOnly();
        }

        private static IReadOnlyList<Piece> AddToGameObjectState(LogicalPiece piece, IReadOnlyList<Piece> gameObjectState)
        {
            List<Piece> intermediateLogicalState = new List<Piece>(gameObjectState);
            intermediateLogicalState.Add(InstantiateAsPiece(piece));
            
            return intermediateLogicalState.AsReadOnly();
        }
        
        private static IReadOnlyList<Piece> RemoveFromGameObjectState(LogicalPiece piece, IReadOnlyList<Piece> gameObjectState)
        {
            Piece toRemove = gameObjectState.First(physical => physical.ToLogicalPiece().Equals(piece));
            Object.Destroy(toRemove.gameObject);

            List<Piece> intermediatePhysicalState = new List<Piece>(gameObjectState);
            intermediatePhysicalState.RemoveAll(physical => physical.ToLogicalPiece().Equals(piece));
            
            return intermediatePhysicalState.AsReadOnly();
        }

        private static Piece InstantiateAsPiece(LogicalPiece piece)
        {
            // this has the very intended sideeffect of actually creating the Piece in the GameWorld.
            // Should be I/O but it isn't
            return Object
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