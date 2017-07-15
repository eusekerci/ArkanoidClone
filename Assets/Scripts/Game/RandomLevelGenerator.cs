using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace Arkanoid
{
    public class RandomLevelGenerator
    {
        private string[] _level;
        private int[] _complexityGraph;
        private int _totalComplexity;

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

            CalculateLevelComplexity();

            return _level;
        }

        public string[] GenerateLevelByComplexity(int complexity)
        {
            return null;
        }

        public string[] GenerateLevelByComplexity(int complexity, int brickDensity, int unbreakableDensity)
        {
            return null;
        }

        private bool IsLevelAchieveable()
        {
            return true;
        }

        public int CalculateLevelComplexity()
        {
            _complexityGraph = new int[_rowCount*_columnCount];
            List<int> searchPositions = new List<int>();

            for (int i = 0; i < _rowCount * _columnCount; i++)
            {
                if (_level[i] == Utils.BrickToString(BrickType.Empty))
                {
                    _complexityGraph[i] = 0;
                }
                else if (_level[i] == Utils.BrickToString(BrickType.Unbreakable))
                {
                    _complexityGraph[i] = 15;
                }
                else
                {
                    _complexityGraph[i] = 99999;
                    searchPositions.Add(i);
                }
            }

            //Now we want to use BFS algorithm 
            Queue<int> bfs = new Queue<int>();
            int _currentIndex;

            //Set the breakables of 15th row start point with complexity 1
            for (int i = (_rowCount - 1) * _columnCount; i < _columnCount*_rowCount; i++)
            {
                if (_level[i] == Utils.BrickToString(BrickType.Basic))
                {
                    _complexityGraph[i] = 1;
                    searchPositions.Remove(i);
                    bfs.Enqueue(i);
                }
            }

            while (bfs.Count > 0)
            {
                _currentIndex = bfs.Dequeue();

                List<int> neighbours = Utils.GetNeighbours(_currentIndex);
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (_level[neighbours[i]] == Utils.BrickToString(BrickType.Basic) && searchPositions.Count > 0 && searchPositions.Contains(neighbours[i]))
                    {
                        _complexityGraph[neighbours[i]] = _complexityGraph[_currentIndex] + 1;
                        searchPositions.Remove(neighbours[i]);
                        bfs.Enqueue(neighbours[i]);
                    }
                }
            }

            for (int i = 0; i < _rowCount; i++)
            {
                string asd = "";
                for (int j = 0; j < _columnCount; j++)
                {
                    asd += _complexityGraph[i * _columnCount + j].ToString() + " ";
                }
                Debug.Log(asd);
            }

            return 0;
        }
    }
}