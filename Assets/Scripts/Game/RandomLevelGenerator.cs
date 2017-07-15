using System;
using System.Collections;
using System.Collections.Generic;

namespace Arkanoid
{
    public class RandomLevelGenerator
    {
        private string[] _level;

        private const int _rowCount = 15;
        private const int _columnCount = 10;

        private Random _rand;

        public RandomLevelGenerator()
        {
            _level = new string[_rowCount * _columnCount];
            _rand = new Random();
        }

        //Totally Random
        public string[] GenerateRandomLevel()
        {
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    int ind = _rand.Next(Enum.GetNames(typeof(BrickType)).Length);
                    _level[i * _columnCount + j] = ind.ToString();
                }
            }

            return _level;
        }
    }
}