using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Arkanoid
{
    public static class Utils
    {
        public static Random Random;

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

        public static bool IsNeighbour(int tileA, int tileB, int rowCount = 15, int columnCount = 10)
        {
            if (tileA - 1 == tileB && tileB % columnCount != 9)
                return true;
            if (tileA + 1 == tileB && tileB % columnCount != 0)
                return true;
            if (tileA + columnCount == tileB)
                return true;
            if (tileA - columnCount == tileB)
                return true;

            return false;
        }

        public static List<int> GetNeighbours(int tileA, int rowCount = 15, int columnCount = 10)
        {
            List<int> neighbours = new List<int>();

            if ((tileA - 1) % columnCount != 9 && tileA - 1 > 0)
                neighbours.Add(tileA-1);
            if ((tileA + 1) % columnCount != 0)
                neighbours.Add(tileA+1);
            if (tileA + columnCount < rowCount*columnCount)
                neighbours.Add(tileA + columnCount);
            if (tileA - columnCount >= 0)
                neighbours.Add(tileA - columnCount);

            return neighbours;
        }

        public static Type[] GetChildrenTypesOf<T>()
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(T).IsAssignableFrom(assemblyType) && assemblyType != typeof(T)
                select assemblyType).ToArray();
        }
    }
}