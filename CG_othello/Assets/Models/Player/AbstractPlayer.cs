using System.Collections.Generic;
using System.Linq;
using Models.Board;
using UnityEngine;

namespace Models.Player
{
    public abstract class AbstractPlayer : IPlayer
    {
        protected readonly PlayerColor Color;
        protected readonly IReadOnlyList<Move> PotentialMoves;
        protected readonly IReadOnlyList<Move> MyMoves;
        private readonly bool _hasPassed;

        protected AbstractPlayer(PlayerColor color, IReadOnlyList<Move> potentialMoves, bool hasPassed)
        {
            Color = color;
            PotentialMoves = potentialMoves;
            MyMoves = potentialMoves.Where(move => move.Origin.Color.Equals(Color)).ToList();
            _hasPassed = hasPassed;
        }
        
        public bool HasNextMove() => 0 < MyMoves.Count;

        public bool HasPassed() => _hasPassed;
        
        public bool CanPlayOn(int x, int z) => 
            MyMoves.Count(move => move.Origin.X == x && move.Origin.Z == z) > 0;

        public IReadOnlyList<Move> GetPotentialMoves() => MyMoves;

        public PlayerColor GetColor() => Color;

        protected static List<Move> CalculatePotentialMoves(IReadOnlyList<LogicalPiece> state)
        {
            var result = new List<Move>();
            
            foreach (var alreadyPlacedPiece in state)
            {
                var directions = GetOpposingAdjacentsOf(alreadyPlacedPiece, state);
                var moves = GetMovesFrom(alreadyPlacedPiece, directions, state);
                
                result.AddRange(moves);
            }

            return result;
        }

        private static List<LogicalPiece> GetOpposingAdjacentsOf(LogicalPiece existingPiece, IReadOnlyList<LogicalPiece> state)
        {
            IEnumerable<int> Range(int pos) => Enumerable.Range(pos - 1, 3);
            LogicalPiece OpposingPieceAt(int x, int z) => new LogicalPiece(x, z, existingPiece.Color.Opposing());
            
            List<LogicalPiece> adjacents = new List<LogicalPiece>(8);

            foreach (var row in Range(existingPiece.X))
            {
                foreach (var column in Range(existingPiece.Z))
                {
                    var enemyPieceAsDirection = OpposingPieceAt(row, column);
                    if (state.Contains(enemyPieceAsDirection)) adjacents.Add(enemyPieceAsDirection);
                }
            }

            return adjacents;
        }

        private static List<Move> GetMovesFrom(LogicalPiece bound, List<LogicalPiece> directions, IReadOnlyList<LogicalPiece> state)
        {
            List<Move> moves = new List<Move>();
            
            foreach (LogicalPiece direction in directions)
            {
                var (possiblePieceForMove, flippedPieces) = 
                    GetPieceAtEndOfLineAndDistanceFrom(bound, direction, state);

                if (!state.Contains(possiblePieceForMove) && InsideBounds(possiblePieceForMove)) 
                    moves.Add(new Move(possiblePieceForMove, bound, flippedPieces));
            }

            return moves;
        }

        private static (LogicalPiece, int) GetPieceAtEndOfLineAndDistanceFrom(
            LogicalPiece origin, 
            LogicalPiece direction, 
            IReadOnlyList<LogicalPiece> state)
        {
            int AdvanceOneStepFrom(int i) => i + (i < 0 ? -1 : i == 0 ? 0 : 1); // increment or decrement depending on the direction
            LogicalPiece NewLogicalPieceAt(int row, int column) => 
                new LogicalPiece(origin.X + row, origin.Z + column, origin.Color.Opposing());
            
            var (rowDirection, colDirection) = GetSlopeFrom(origin, direction);
            int flipped = 0;
            
            LogicalPiece next = NewLogicalPieceAt(rowDirection, colDirection);
            
            while (state.Contains(next))
            {
                rowDirection = AdvanceOneStepFrom(rowDirection);
                colDirection = AdvanceOneStepFrom(colDirection);
                next = NewLogicalPieceAt(rowDirection, colDirection);
                flipped++;
            }

            return (new LogicalPiece(next.X, next.Z, origin.Color), flipped);
        }

        private static (int, int) GetSlopeFrom(LogicalPiece origin, LogicalPiece direction)
        {
            // this works because we know that destination and adjacent will always be at most one step apart
            int GetDirectionFrom(int start, int end) => start < end ? 1 : end == start ? 0 : -1;
            
            return (GetDirectionFrom(origin.X, direction.X), GetDirectionFrom(origin.Z, direction.Z));
        }

        private static bool InsideBounds(LogicalPiece piece)
        {
            bool InsideBounds(int pos) => pos >= 0 && pos < Game.Instance.BoardLength;
            
            return InsideBounds(piece.X) && InsideBounds(piece.Z);
        }
        
        public abstract List<Move> GetNextMove();
        public abstract IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state);
        public abstract IPlayer WithPass();
    }
}