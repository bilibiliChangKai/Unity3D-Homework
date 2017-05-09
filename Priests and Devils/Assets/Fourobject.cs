using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.Position
{
    public class Devil
    {
        public static Vector3 leftFirstPosition = new Vector3(-22, 2, 0);
        public static Vector3 rightFirstPosition = new Vector3(10, 2, 0);
        public static Vector3 spaceWithTwo = new Vector3(2, 0, 0);
    }

    public class Priest
    {
        public static Vector3 leftFirstPosition = new Vector3(-15, 2, 0);
        public static Vector3 rightFirstPosition = new Vector3(17, 2, 0);
        public static Vector3 spaceWithTwo = new Vector3(2, 0, 0);
    }

    public class Shore
    {
        public static Vector3 leftPosition = new Vector3(-16, -1, 0);
        public static Vector3 rightPosition = new Vector3(16, -1, 0);
    }

    public class Boat
    {
        public static Vector3 leftPosition = new Vector3(-6, -0.5f, 0);
        public static Vector3 rightPosition = new Vector3(6, -0.5f, 0);
        public static Vector3 leftSeat = new Vector3(-1.5f, 1.5f, 0);
        public static Vector3 rightSeat = new Vector3(1.5f, 1.5f, 0);
        public static float Speed = 10f;
    }
}
