namespace Bidirectional8Puzzle
{
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        None = 4
    }
    static class DirectionExtensions
    {
        public static Direction Opposite(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
            }
            return Direction.None;
        }
    }
}
