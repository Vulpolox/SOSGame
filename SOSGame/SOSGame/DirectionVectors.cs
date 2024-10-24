using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    // static class for holding vector representations of the 8 directions
    public static class DirectionVectors
    {
        // directions
        public static Vector2 Up { get; } = new Vector2(-1, 0);
        public static Vector2 Down { get; } = new Vector2(1, 0);
        public static Vector2 Left { get; } = new Vector2(0, -1);
        public static Vector2 Right { get; } = new Vector2(0, 1);
        public static Vector2 UpLeft { get; } = new Vector2(-1, -1);
        public static Vector2 UpRight { get; } = new Vector2(-1, 1);
        public static Vector2 DownLeft { get; } = new Vector2(1, -1);
        public static Vector2 DownRight { get; } = new Vector2(1, 1);


        // method for returning list of all directions
        public static List<Vector2> GetAllDirections()
        {
            return new List<Vector2>
            {
                Up, Down, Left, Right,
                UpLeft, UpRight,
                DownLeft, DownRight
            };
        }
    }
}
