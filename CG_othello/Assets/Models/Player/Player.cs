using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Board;
using UnityEngine;

namespace Models.Player
{
    public abstract class Player : MonoBehaviour
    {
        
        public abstract Tuple<int, int, PlayerColor> GetNextMove();
        public abstract List<Move> GetPotentialMoves();

        public abstract void SetNextMove(int x, int z);

        protected List<Move> CalculatePotentialMoves(List<Piece> state)
        {
            List<Move> result = new List<Move>();

            foreach (Piece piece in state)
            {
                List<Piece> directions = GetOpposingAdjacentsOf(piece, state);
                List<Move> moves = GetMovesFrom(piece, directions, state);
                
                result.AddRange(moves);
            }

            return result;
        }

        private List<Piece> GetOpposingAdjacentsOf(Piece piece, List<Piece> state)
        {
            IEnumerable<int> Range(int pos) => Enumerable.Range(pos - 1, pos + 1);
            Piece OpposingPieceAt(int x, int z)
            {
                return Game.Instance.gameObject.AddComponent<Piece>().Init(x, z, piece.Color.Opposing());
                
            }


            List<Piece> directions = new List<Piece>();
            
            foreach (var row in Range(piece.X))
            {
                foreach (var column in Range(piece.Z))
                {
                    Piece opposingPiece = OpposingPieceAt(row, column);
                    if (state.Contains(opposingPiece)) directions.Add(opposingPiece);
                }
            }

            return directions;
        }

        private List<Move> GetMovesFrom(Piece origin, List<Piece> directions, List<Piece> state)
        {
            int DirectionFrom(int start, int end) => start.CompareTo(end);

            List<Move> moves = new List<Move>();
            
            foreach (Piece adjacent in directions)
            {
                int rowDirection = DirectionFrom(origin.X, adjacent.X);
                int colDirection = DirectionFrom(origin.Z, adjacent.Z);

                int Advance(int i) => i + 0.CompareTo(i);
                Func<int, int, Piece> GetPieceAtPosition = (row, column) => 
                    gameObject.GetComponent<Piece>().Init(origin.X + row, origin.Z + column, origin.Color.Opposing());

                int flipped = 0;
                Piece next = GetPieceAtPosition(rowDirection, colDirection);

                while (state.Contains(next))
                {
                    rowDirection = Advance(rowDirection);
                    colDirection = Advance(colDirection);
                    next = GetPieceAtPosition(rowDirection, colDirection);
                    flipped++;
                }
                
                Piece possiblePiece = gameObject.GetComponent<Piece>().Init(next.X, next.Z, origin.Color);

                if (!state.Contains(possiblePiece) && InsideBounds(possiblePiece) && flipped > 0)
                {
                    moves.Add(new Move(origin, possiblePiece, flipped));   
                }
            }

            return moves;
        }

        private bool InsideBounds(Piece piece)
        {
            int length = Game.Instance.BoardLength;
            bool InsideBounds(int pos) { return pos >= 0 && pos < length; }
            
            return InsideBounds(piece.X) && InsideBounds(piece.Z);
        }
    }
}