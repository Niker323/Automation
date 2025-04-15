using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Automation
{
    public class Side2D
    {
        public const int NumberOfSides = 4;
        public const int indexUP = 0;
        public const int indexRIGHT = 1;
        public const int indexDOWN = 2;
        public const int indexLEFT = 3;

        int index;
        int oppositeIndex;
        Vector2 normal;
        Vector2Int normali;
        string code;

        Side2D(string code, int index, int oppositeIndex, Vector2Int normal)
        {
            this.index = index;
            this.oppositeIndex = oppositeIndex;
            this.normal = normal;
            normali = normal;
            this.code = code;
        }

        public static readonly Side2D UP = new Side2D("up", 0, 2, new Vector2Int(0, 1));
        public static readonly Side2D RIGHT = new Side2D("right", 1, 3, new Vector2Int(1, 0));
        public static readonly Side2D DOWN = new Side2D("down", 2, 0, new Vector2Int(0, -1));
        public static readonly Side2D LEFT = new Side2D("left", 3, 1, new Vector2Int(-1, 0));

        public static readonly Side2D[] ALLSIDES = new Side2D[NumberOfSides] { UP, RIGHT, DOWN, LEFT };

        public Vector2 Normal { get => normal; }
        public Vector2Int Normali { get => normali; }
        public string Code { get => code; }
        public int Index { get => index; }
        public Side2D Opposite { get => ALLSIDES[oppositeIndex]; }

        public static Side2D FromCode(string code)
        {
            code = code?.ToLowerInvariant();
            switch (code)
            {
                case "up":
                    return UP;
                case "right":
                    return RIGHT;
                case "down":
                    return DOWN;
                case "left":
                    return LEFT;
                default:
                    return null;
            }
        }

        public static Side2D FromFirstLetter(string code)
        {
            return FromFirstLetter(code[0]);
        }

        public static Side2D FromFirstLetter(char code)
        {
            switch (code)
            {
                case 'u':
                    return UP;
                case 'r':
                    return RIGHT;
                case 'd':
                    return DOWN;
                case 'l':
                    return LEFT;
                default:
                    return null;
            }
        }

        public Side2D GetCW()
        {
            return ALLSIDES[(index + 1) % 4];
        }

        public Side2D GetCCW()
        {
            return ALLSIDES[(index + 3) % 4];
        }

        public override string ToString()
        {
            return code;
        }
    }
}
