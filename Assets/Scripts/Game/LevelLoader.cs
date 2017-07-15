using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Arkanoid
{
    public class LevelLoader : MonoBehaviour
    {
        private string[] _levelMap;

        private const int _rowCount = 15;
        private const int _columnCount = 10;
        private Transform _brickRoot;
        private const float _firstRowY = 4.2f;
        private const float _firstColumnX = -2.25f;
        private const float _rowOffset = -0.3f;
        private const float _columnOffset = 0.5f;

        public Transform BasicBrick;

        void Start()
        {
            _brickRoot = GameObject.Find("BrickRoot").transform;

            _levelMap = new string[]
            {
                "0", "1", "0", "1", "0", "0", "0", "0", "0", "0",
                "0", "0", "1", "0", "0", "0", "0", "0", "0", "1",
                "0", "0", "0", "1", "0", "1", "1", "0", "0", "0",
                "0", "0", "1", "1", "1", "0", "0", "1", "1", "1",
                "0", "0", "0", "0", "0", "1", "0", "1", "0", "0",
                "0", "0", "0", "1", "0", "0", "1", "0", "0", "0",
                "0", "0", "1", "0", "1", "0", "1", "1", "0", "1",
                "0", "1", "0", "0", "0", "1", "0", "0", "1", "0",
                "0", "0", "1", "0", "1", "0", "1", "0", "1", "1",
                "0", "0", "0", "1", "0", "0", "0", "1", "0", "0",
                "0", "0", "1", "0", "1", "0", "1", "0", "1", "0",
                "0", "0", "0", "0", "0", "1", "0", "0", "0", "0",
                "1", "0", "1", "0", "1", "0", "1", "1", "1", "0",
                "0", "0", "0", "0", "0", "0", "0", "1", "0", "0",
                "0", "0", "1", "0", "0", "0", "0", "0", "1", "0"
            };

            MessageBus.OnEvent<GameStartedEvent>().Subscribe(evnt =>
            {
                RandomLevelGenerator _levelGenerator = new RandomLevelGenerator();
                //_levelMap = _levelGenerator.GenerateRandomLevel();
                _levelMap = _levelGenerator.GenerateRandomLevel(100, 20);
                InitiliazeLevel();
            });
        }

        void Update()
        {

        }

        public void InitiliazeLevel()
        {
            for (int i = 0; i < _rowCount; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    if (_levelMap[i * _columnCount + j] != Utils.BrickToString(BrickType.Empty))
                    {
                        int type = Int32.Parse(_levelMap[i * _columnCount + j]);
                        GameObject go = BrickPools.Pools[(BrickType)type].Get();
                        go.transform.position = new Vector3(_firstColumnX + j * _columnOffset, _firstRowY + i * _rowOffset, 0);
                        go.transform.SetParent(_brickRoot);
                    }
                }
            }
        }
    }
}
