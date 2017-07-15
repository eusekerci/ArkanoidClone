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

        //Random Level by Density
        //BrickDensity and UnbreakableDensity must be between 0-100
        //BrickDensity = bricks/(row*column)
        //UnbreakableDensity = unbreakables/bricks
        public string[] GenerateRandomLevel(int brickDensity, int unbreakableDensity)
        {
            int brickCount = (150 * brickDensity) / 100;
            int unbreakableCount = (brickCount * unbreakableDensity) / 100;

            for (int i = 0; i < _rowCount * _columnCount; i++)
            {
                _level[i] = Utils.BrickToString(BrickType.Empty);
            }

            while(brickCount > 0)
            {
                int randomIndex = _rand.Next(_rowCount * _columnCount);

                if (_level[randomIndex] != Utils.BrickToString(BrickType.Empty))
                {
                    continue;
                }

                if (unbreakableCount > 0)
                {
                    _level[randomIndex] = Utils.BrickToString(BrickType.Unbreakable);
                    unbreakableCount--;
                }
                else
                {
                    _level[randomIndex] = Utils.BrickToString(BrickType.Basic);
                }

                brickCount--;
            }

            return _level;
        }

        //TODO
        private bool IsLevelAchieveable()
        {
            return true;
        }

        public int CalculateLevelComplexity()
        {
            return 0;
        }

        public string[] GenerateLevelByComplexity(int complexity)
        {
            return null;
        }

        public string[] GenerateLevelByComplexity(int complexity, int brickDensity, int unbreakableDensity)
        {
            return null;
        }
    }
}