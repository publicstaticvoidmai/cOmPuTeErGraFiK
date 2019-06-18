using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Board;

namespace Models.Player
{
    public interface IPlayer
    {
        bool HasNextMove();
        bool HasPassed();

        IReadOnlyList<Move> GetPotentialMoves();
        Task<List<Move>> GetNextMove();
        PlayerColor GetColor();
        
        Task<IPlayer> WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state);
        IPlayer WithPass();
    }
}