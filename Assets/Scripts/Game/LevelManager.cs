using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Arkanoid
{
    public class LevelIsCompleted : ArkEvent { }

    public class LevelManager : MonoBehaviour
    {
        private string[] _levelMap;

        private const int _rowCount = 15;
        private const int _columnCount = 10;
        private Transform _brickRoot;
        private const float _firstRowY = 4.2f;
        private const float _firstColumnX = -2.25f;
        private const float _rowOffset = -0.3f;
        private const float _columnOffset = 0.5f;
        
        private Dictionary<BrickType, int> _brickCount;

        void Start()
        {
            _brickRoot = GameObject.Find("BrickRoot").transform;
            _brickCount = new Dictionary<BrickType, int>();
            foreach (BrickType type in Enum.GetValues(typeof(BrickType)))
            {
                _brickCount.Add(type, 0);
            }

            MessageBus.OnEvent<GameIsInitiliazed>().Subscribe(evnt =>
            {
                RandomLevelGenerator _levelGenerator = new RandomLevelGenerator();
                //_levelMap = _levelGenerator.GenerateRandomLevel();
                _levelMap = _levelGenerator.GenerateRandomLevel(10, 0);
                InitiliazeLevel();
            });

            MessageBus.OnEvent<BrickIsDestroyed>().Subscribe(evnt =>
            {
                _brickCount[evnt.BType]--;
                if (_brickCount[evnt.BType] <= 0)
                {
                    Debug.Log("Level is Completed");
                    MessageBus.Publish(new LevelIsCompleted());
                }
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
                    int ind = i * _columnCount + j;
                    if (_levelMap[ind] != Utils.BrickToString(BrickType.Empty))
                    {
                        int type = Int32.Parse(_levelMap[ind]);
                        GameObject go = BrickPools.Pools[(BrickType)type].Get();
                        go.transform.position = new Vector3(_firstColumnX + j * _columnOffset, _firstRowY + i * _rowOffset, 0);
                        go.transform.SetParent(_brickRoot);
                    }

                    _brickCount[Utils.StringToBrick(_levelMap[ind])]++;
                }
            }
        }
    }
}
