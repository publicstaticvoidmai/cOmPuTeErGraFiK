namespace Models.Board
{
    public class Move
    {
        public readonly Piece Origin;
        public readonly Piece Destination;
        public readonly int Flipped;

        public Move(Piece origin, Piece destination, int flipped)
        {
            Origin = origin;
            Destination = destination;
            Flipped = flipped;
        }
    }
}