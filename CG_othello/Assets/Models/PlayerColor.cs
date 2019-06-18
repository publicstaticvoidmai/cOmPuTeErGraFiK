namespace Models
{
    public enum PlayerColor { White, Black }
    static class PlayerColorMethods
    {
        public static PlayerColor Opposing(this PlayerColor color)
        {
            return color == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
        }
    }
}