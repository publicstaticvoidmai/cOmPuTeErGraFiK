using System.Collections.Generic;
using Models.Board;

namespace Models.Player
{
    public interface IPlayer
    {
        bool HasNextMove();
        bool HasPassed();

        IReadOnlyList<Move> GetPotentialMoves();
        List<Move> GetNextMove();
        PlayerColor GetColor();
        
        IPlayer WithCalculatedPotentialMovesFrom(IReadOnlyList<LogicalPiece> state);
        IPlayer WithPass();
    }
}