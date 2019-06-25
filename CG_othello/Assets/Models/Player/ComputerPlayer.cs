using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Models.Board;

namespace Models.Player
{
    public class ComputerPlayer : AbstractPlayer
    {
        public static ComputerPlayer Create(PlayerColor color)
        {
            return new ComputerPlayer(color, new List<Move>(), false);
        }

        public override List<Move> GetNextMove()
        {
            Thread.Sleep(200);
            var origin = MyMoves.First().Origin;
            return MyMoves
                .Where(move => move.Origin.Equals(origin))
                .ToList();
        }

        public override IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state)
        {
            return new ComputerPlayer(Color, CalculatePotentialMoves(state), false);
        }
        
        public override IPlayer WithPass()
        {
            return new ComputerPlayer(Color, PotentialMoves, true);
        }

        private ComputerPlayer(PlayerColor color, IReadOnlyList<Move> potentialMoves, bool hasPassed) : base(color, potentialMoves, hasPassed)
        {
        }
    }
}