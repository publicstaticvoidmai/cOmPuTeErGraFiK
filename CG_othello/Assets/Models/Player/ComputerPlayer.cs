using System;
using System.Collections.Generic;
using System.Linq;
using Models.Board;

using UnityEngine;

namespace Models.Player
{
    public class ComputerPlayer : AbstractPlayer
    {
        private const double MovabilityWeight = 0.8;
        private const double GainWeight = 0.6;
        private const double EdgeWeight = 0.9;

        // depth could be adjusted if so desired. complexity is n^n though.
        private readonly int _depth;

        private readonly IReadOnlyList<LogicalPiece> _lastState;

        public static ComputerPlayer Create(PlayerColor color, int searchDepth = 2)
        {
            String complexity = searchDepth + "^" + searchDepth;
            if (searchDepth > 3) Debug.Log(
                "The Game will be incredibly slow, " +
                "please refrain from setting searchdepths bigger than 3. " +
                "You selected " + searchDepth + ", the complexity will be " + complexity
            );
            return new ComputerPlayer(color, new List<Move>(), false, new List<LogicalPiece>(), searchDepth);
        }

        class Reward : IComparable {
            private readonly long _movability;
            private readonly double _gain;

            public Reward(long movability, double gain) {
                _movability = movability;
                _gain = gain;
            }

            public int CompareTo(object obj)
            {
                if (obj is Reward other)
                {
                    return Math.Sign(
                        MovabilityWeight * (_movability - other._movability) +
                        GainWeight * (_gain - other._gain)
                    );
                }
                return 0;
            }
        }
        
        private double GetReward(List<Move> move)
        {
            double reward = 0;
            foreach (var singleMove in move)
            {
                var edgeLength = Game.Instance.BoardLength;
                int distanceMiddleRow = Math.Abs(singleMove.Played.X - edgeLength / 2);
                int distanceMiddleColumn = Math.Abs(singleMove.Played.Z - edgeLength / 2);
                // the further we stray from the middle the less important it gets to flip a lot of fields
                double weightFlipped = 1 - EdgeWeight * (distanceMiddleColumn + distanceMiddleRow) / edgeLength;

                reward += weightFlipped * singleMove.Flipped;
            }

            return reward;
        }

        private long GetMovability(List<Move> move, IReadOnlyList<LogicalPiece> state, int depth)
        {
            if (depth == 0) return 0; // what even is tail-call optimisation. Thanks c#
            
            Board.Board unrealBoard = new Board.Board(state);
            Board.Board board = unrealBoard.With(move); // advance one step into the future to see what it has in stock for us

            PlayerColor ownColor = move.First().Played.Color;
            PlayerColor opposingColor = ownColor.Opposing();
            
            List<Move> opponentMoves = CalculatePotentialMoves(board.LogicalState)
                .Where(selected => 
                    selected.Played.Color.Equals(opposingColor)
                )
                .ToList();
            List<Move> predictedOpponentMove = GetHighestRewardMove(opponentMoves, board.LogicalState, depth - 1);
            Board.Board predictedNextState = board.With(predictedOpponentMove);

            var next = CalculatePotentialMoves(predictedNextState.LogicalState);
            
            return MovabilityFor(next, ownColor) - MovabilityFor(next, opposingColor) * (depth / 10);
        }
        
        private long MovabilityFor(List<Move> moves, PlayerColor color)
        {
            return moves.Count(selected => selected.Played.Color.Equals(color));
        }

        private List<Move> GetHighestRewardMove(List<Move> nextPossibleMoves, IReadOnlyList<LogicalPiece> state, int depth)
        {
            List<Move> bestMove = new List<Move>();
            Reward highestReward = new Reward(int.MinValue, int.MinValue);
            foreach (var entry in ZipWithReward(nextPossibleMoves, state, depth))
            {
                Reward currentReward = entry.Value;
                if (highestReward.CompareTo(currentReward) < 0 ) {
                    highestReward = currentReward;
                    bestMove = nextPossibleMoves.FindAll(piece => piece.Played.Equals(entry.Key));
                }
            }

            return bestMove;
        }

        private Dictionary<LogicalPiece, Reward> ZipWithReward(List<Move> moves, IReadOnlyList<LogicalPiece> state, int depth)
        {
            Dictionary<LogicalPiece, Reward> likelyhood = new Dictionary<LogicalPiece, Reward>();
            var actualMoves = moves.GroupBy(selected => selected.Played);
            foreach (var groupedMove in actualMoves)
            {
                List<Move> move = new List<Move>(groupedMove.AsEnumerable());
                likelyhood.Add(move.First().Played, new Reward(GetMovability(move, state, depth), GetReward(move)));
            }

            return likelyhood;
        }

        public override List<Move> GetNextMove()
        {
            return GetHighestRewardMove(new List<Move>(MyMoves), _lastState, _depth); //change
        }

        public override IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state)
        {
            return new ComputerPlayer(Color, CalculatePotentialMoves(state), false, state, _depth);
        }
        
        public override IPlayer WithPass()
        {
            return new ComputerPlayer(Color, PotentialMoves, true, new List<LogicalPiece>(), _depth);
        }

        private ComputerPlayer(
            PlayerColor color, 
            IReadOnlyList<Move> potentialMoves, 
            bool hasPassed, 
            IReadOnlyList<LogicalPiece> state, int depth) : base(color, potentialMoves, hasPassed)
        {
            _lastState = state;
            _depth = depth;
        }
    }
}