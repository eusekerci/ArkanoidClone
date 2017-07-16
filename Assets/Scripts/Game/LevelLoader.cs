using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Arkanoid
{
    public class LevelIsCompleted : ArkEvent
    {
        public bool Succeed;
    }

    public class LevelIsLoaded : ArkEvent
    {
        public int Complexity;
    }

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
        
        private Dictionary<BrickType, int> _brickCount;
        private RandomLevelGenerator _levelGenerator;

        public void Init()
        {
            _levelGenerator = new RandomLevelGenerator();
            _brickRoot = GameObject.Find("BrickRoot").transform;
            _brickCount = new Dictionary<BrickType, int>();
            foreach (BrickType type in Enum.GetValues(typeof(BrickType)))
            {
                _brickCount.Add(type, 0);
            }

            MessageBus.OnEvent<GameIsInitiliazed>().Subscribe(evnt =>
            {
                ClearLevel();

                if(!evnt.SameLevel)
                    _levelMap = _levelGenerator.GenerateRandomLevel(evnt.BrickDensity, evnt.UnbreakableDensity);

                InitiliazeLevel();
                MessageBus.Publish(new LevelIsLoaded()
                {
                    Complexity =  _levelGenerator.GetComplexity()
                });
            });

            MessageBus.OnEvent<BrickIsDestroyed>().Subscribe(evnt =>
            {
                OnBrickDestroyed(evnt.BType);
            });
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

        public void ClearLevel()
        {
            if (_brickRoot.childCount > 0)
            {
                foreach (Brick brck in _brickRoot.GetComponentsInChildren<Brick>())
                {
                    brck.Clear();
                }
            }

            foreach (BrickType type in Enum.GetValues(typeof(BrickType)))
            {
                _brickCount[type] = 0;
            }
        }

        private void OnBrickDestroyed(BrickType type)
        {
            _brickCount[type]--;
            if (_brickCount[type] <= 0)
            {
                Debug.Log("Level is Completed");
                MessageBus.Publish(new LevelIsCompleted()
                {
                    Succeed = true
                });
            }
        }
    }
}
