using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Board;

namespace Models.Player
{
    public class HumanPlayer : AbstractPlayer
    {
        public static HumanPlayer Create(PlayerColor color)
        {
            return new HumanPlayer(color, new List<Move>(), false);
        }
        
        private int? _nextX;
        private int? _nextZ;

        public override List<Move> GetNextMove()
        {
            if (!_nextX.HasValue || !_nextZ.HasValue) return new List<Move>();
            
            LogicalPiece selected = new LogicalPiece(_nextX.Value, _nextZ.Value, Color);
            _nextX = null;
            _nextZ = null;
            
            // This is a bold assumption: For we check if the field exists in the tile, we know that we should only
            // get selected pieces that are actually in the set of potentialMoves
            return MyMoves
                .Where(move => move.Origin.Equals(selected))
                .ToList();
        }

        public override IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state)
        {
            return new HumanPlayer(Color, CalculatePotentialMoves(state), false);
        }

        public override IPlayer WithPass()
        {
            return new HumanPlayer(Color, PotentialMoves, true);
        }

        public void SetNextMove(int x, int z)
        {
            _nextX = x;
            _nextZ = z;
        }

        private HumanPlayer(PlayerColor color, IReadOnlyList<Move> potentialMoves, bool hasPassed) : base(color, potentialMoves, hasPassed)
        {
        }
    }
}