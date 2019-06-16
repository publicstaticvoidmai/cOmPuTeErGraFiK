using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Board;
using UnityEngine;

namespace Models.Player
{
    public abstract class Player
    {
        public abstract Tuple<int, int, PlayerColor> GetNextMove();
        public abstract List<Move> GetPotentialMoves();

        protected List<Move> CalculatePotentialMoves(Piece[,] state)
        {
            List<Move> result = new List<Move>();
            
            foreach (var piece in state)
            {
                List<Piece> directions = GetOpposingAdjacentsOf(piece, state);
                List<Move> moves = GetMovesFrom(piece, directions, state);
            }

            return result;
        }

        private List<Piece> GetOpposingAdjacentsOf(Piece piece, Piece[,] state)
        {
            IEnumerable<int> Range(int pos) => Enumerable.Range(pos - 1, pos + 1);
            ArrayList stateCopy = new ArrayList(state);
            Piece OpposingPieceAt(int x, int z) => new Piece(x, z, piece.Color.Opposing());
            
            List<Piece> directions = new List<Piece>();
            
            foreach (var row in Range(piece.x)) // TODO zu list ding
            {
                foreach (var column in Range(piece.z))
                {
                    Piece opposingPiece = OpposingPieceAt(row, column);
                    if (stateCopy.Contains(opposingPiece)) directions.Add(opposingPiece);
                }
            }

            return directions;
        }

        private List<Move> GetMovesFrom(Piece origin, List<Piece> directions, Piece[,] state)
        {
            int DirectionFrom(int start, int end) => start.CompareTo(end);
            ArrayList stateCopy = new ArrayList(state);

            List<Move> moves = new List<Move>();
            
            foreach (Piece adjacent in directions)
            {
                int rowDirection = DirectionFrom(origin.x, adjacent.x);
                int colDirection = DirectionFrom(origin.z, adjacent.z);

                int Advance(int i) => i + 0.CompareTo(i);
                Piece GetPieceAtPosition(int row, int column) => 
                    new Piece(origin.x + row, origin.z + column, origin.Color.Opposing());

                int flipped = 0;
                Piece next = GetPieceAtPosition(rowDirection, colDirection);

                while (stateCopy.Contains(next))
                {
                    rowDirection = Advance(rowDirection);
                    colDirection = Advance(colDirection);
                    next = GetPieceAtPosition(rowDirection, colDirection);
                    flipped++;
                }
                
                Piece possiblePiece = new Piece(next.x, next.z, origin.Color);

                if (!stateCopy.Contains(possiblePiece) && flipped > 0)
                {
                    moves.Add(new Move(origin, possiblePiece, flipped));   
                }
            }

            return moves;
        }
    }
}