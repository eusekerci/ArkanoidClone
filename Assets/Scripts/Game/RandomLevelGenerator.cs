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

            //If level not achievable, generate try one more time
            //TODO prevent generating unachievable levels next time
            if (_totalComplexity < 0)
            {
                return GenerateRandomLevel(brickDensity, unbreakableDensity);
            }

            return _level;
        }

        public int CalculateLevelComplexity()
        {
            _complexityGraph = new int[_rowCount*_columnCount];
            List<int> searchPositions = new List<int>();

            for (int i = 0; i < _rowCount * _columnCount; i++)
            {
                if (_level[i] != Utils.BrickToString(BrickType.Unbreakable))
                {
                    _complexityGraph[i] = -1;
                    searchPositions.Add(i);
                }
                else
                {
                    _complexityGraph[i] = 25;
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
                    if (_level[neighbours[i]] != Utils.BrickToString(BrickType.Unbreakable) && searchPositions.Count > 0 && searchPositions.Contains(neighbours[i]))
                    {
                        _complexityGraph[neighbours[i]] = _complexityGraph[_currentIndex] + 1;
                        searchPositions.Remove(neighbours[i]);
                        bfs.Enqueue(neighbours[i]);
                    }
                }
            }

            //Revert the complexity of empty tiles to Zero
            for (int i = 0; i < _rowCount * _columnCount; i++)
            {
                if (_level[i] == Utils.BrickToString(BrickType.Empty))
                {
                    _complexityGraph[i] = 0;
                }
            }

            _totalComplexity = 0;
            for (int i = 0; i < _rowCount; i++)
            {
                //string asd = "";
                for (int j = 0; j < _columnCount; j++)
                {
                    //asd += _complexityGraph[i * _columnCount + j] + " ";
                    if (_complexityGraph[i * _columnCount + j] == -1)
                    {
                        return -1;
                    }
                    _totalComplexity += _complexityGraph[i * _columnCount + j];
                }
                //Debug.Log(asd);
            }

            Debug.Log(_totalComplexity);

            return _totalComplexity;
        }

        public int GetComplexity()
        {
            return _totalComplexity;
        }
    }
}