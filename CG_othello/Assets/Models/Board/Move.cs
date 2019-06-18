namespace Models.Board
{
    public class Move
    {
        public readonly LogicalPiece Origin;
        public readonly LogicalPiece Destination;
        public readonly int Flipped;

        public Move(LogicalPiece origin, LogicalPiece destination, int flipped)
        {
            Origin = origin;
            Destination = destination;
            Flipped = flipped;
        }
    }
}