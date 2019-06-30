using System;
using System.Collections.Generic;
using System.Linq;
using Models.Board;

namespace Models.Player
{
    public class ComputerPlayer : AbstractPlayer
    {
        private const double MovabilityWeight = 0.9;
        private const double GainWeight = 0.7;
        private const double EdgeWeight = 0.8;

        private const int Depth = 3;

        private readonly IReadOnlyList<LogicalPiece> _lastState;
        
        public static ComputerPlayer Create(PlayerColor color)
        {
            return new ComputerPlayer(color, new List<Move>(), false, new List<LogicalPiece>());
        }

        class Reward : IComparable {
            public readonly long Movability;
            public readonly double Gain;

            public Reward(long movability, double gain) {
                Movability = movability;
                Gain = gain;
            }

            
            public int CompareTo(object obj)
            {
                if (obj is Reward other)
                {
                    return Math.Sign(
                        MovabilityWeight * (Movability - other.Movability) +
                        GainWeight * (Gain - other.Gain)
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

                reward += weightFlipped * singleMove.Flipped
                       * distanceMiddleColumn / edgeLength
                       * distanceMiddleRow / edgeLength;
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
            return GetHighestRewardMove(new List<Move>(MyMoves), _lastState, Depth);
        }

        public override IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state)
        {
            return new ComputerPlayer(Color, CalculatePotentialMoves(state), false, state);
        }
        
        public override IPlayer WithPass()
        {
            return new ComputerPlayer(Color, PotentialMoves, true, new List<LogicalPiece>());
        }

        private ComputerPlayer(
            PlayerColor color, 
            IReadOnlyList<Move> potentialMoves, 
            bool hasPassed, 
            IReadOnlyList<LogicalPiece> state
            ) : base(color, potentialMoves, hasPassed)
        {
            _lastState = state;
        }
    }
}