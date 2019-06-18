using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Board;

namespace Models.Player
{
    public class ComputerPlayer : AbstractPlayer
    {
        public static ComputerPlayer Create(PlayerColor color)
        {
            return new ComputerPlayer(color, new List<Move>(), false);
        }
        
        public override async Task<List<Move>> GetNextMove()
        {
            var origin = PotentialMoves.First().Origin;
            return await Task.Run(() => PotentialMoves
                .Where(move => move.Origin.Equals(origin))
                .ToList());
        }

        public override async Task<IPlayer> WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state)
        {
            return new ComputerPlayer(Color, await CalculatePotentialMoves(state), false);
        }
        
        public override IPlayer WithPass()
        {
            return new ComputerPlayer(Color, PotentialMoves, false);
        }

        private ComputerPlayer(PlayerColor color, IReadOnlyList<Move> potentialMoves, bool hasPassed) : base(color, potentialMoves, hasPassed)
        {
        }
    }
}