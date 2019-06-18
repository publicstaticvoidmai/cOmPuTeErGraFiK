namespace Models.Board
{
    public class LogicalPiece
    {
        public readonly int X;
        public readonly int Z;
        public readonly PlayerColor Color;
        
        public LogicalPiece(int x, int z, PlayerColor color)
        {
            X = x;
            Z = z;
            Color = color;
        }

        public override bool Equals(object other)
        {
            return other is LogicalPiece otherPiece && 
                   otherPiece.X == X && 
                   otherPiece.Z == Z && 
                   otherPiece.Color == Color;
        }
    }
}