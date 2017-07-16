using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Arkanoid
{
    public class GameIsInitiliazed : ArkEvent
    {
        public int BrickDensity;
        public int UnbreakableDensity;
    }

    public class GameIsStarted : ArkEvent { }

    public class GameIsOver : ArkEvent { }

    public enum GameMode
    {
        QuestMode = 0,
        EndlessMode = 1,
    }

    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        #endregion

        public bool GameIsStarted = false;
        public GameMode GameMode;
        private List<Vector2> _questModeLevels;
        private int _currentLevel;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            if (GameMode == GameMode.QuestMode)
            {
                _questModeLevels = new List<Vector2>();
                _questModeLevels.Add(new Vector2(10, 0));
                _questModeLevels.Add(new Vector2(20, 10));
                _questModeLevels.Add(new Vector2(30, 20));
                _questModeLevels.Add(new Vector2(40, 30));
                _questModeLevels.Add(new Vector2(50, 40));
                _currentLevel = 0;

                BrickPools.InitiliazeBrickPools();
                MessageBus.Publish(new GameIsInitiliazed()
                {
                   BrickDensity = (int)_questModeLevels[_currentLevel].x,
                   UnbreakableDensity = (int)_questModeLevels[_currentLevel].y,
                });

                MessageBus.OnEvent<LevelIsCompleted>().Subscribe(evnt =>
                {
                    IDisposable d = null;
                    d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
                    {
                        GameIsStarted = false;
                        _currentLevel++;
                        if (_currentLevel < _questModeLevels.Count)
                        {
                            MessageBus.Publish(new GameIsInitiliazed()
                            {
                                BrickDensity = (int) _questModeLevels[_currentLevel].x,
                                UnbreakableDensity = (int) _questModeLevels[_currentLevel].y,
                            });
                        }
                        else
                        {
                            MessageBus.Publish(new GameIsOver());
                        }
                        d.Dispose();
                    });
                });
            }
        }

        void Update()
        {
            if (!GameIsStarted)
            {
#if UNITY_ANDROID || UNITY_IPHONE
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    GameIsStarted = true;
                    MessageBus.Publish(new GameIsStarted());
                }
#else
                if (Input.anyKeyDown)
                {
                    GameIsStarted = true;
                    MessageBus.Publish(new GameIsStarted());
                }
#endif
            }
        }

        public int GetLevelIndex()
        {
            return _currentLevel;
        }
    }
}