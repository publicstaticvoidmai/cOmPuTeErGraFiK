namespace Models.Board
{
    public class Move
    {
        public readonly LogicalPiece Played;
        public readonly LogicalPiece Bound;
        public readonly int Flipped;

        public Move(LogicalPiece played, LogicalPiece bound, int flipped)
        {
            Played = played;
            Bound = bound;
            Flipped = flipped;
        }

        public override string ToString()
        {
            return "Play " + Played + " to " + Bound + " and flip " + Flipped + " Pieces";
        }
    }
}