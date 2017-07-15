using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public static class Utils
    {
        public static string BrickToString(BrickType type)
        {
            return ((int) type).ToString();
        }

        public static int BrickToInt(BrickType type)
        {
            return (int)type;
        }

        public static BrickType StringToBrick(string str)
        {
            return (BrickType) Int32.Parse(str);
        }

        public static BrickType IntToBrick(int ind)
        {
            return (BrickType)ind;
        }
    }
}