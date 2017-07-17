using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Examples;

namespace Arkanoid
{
    public class GameIsInitiliazed : ArkEvent
    {
        public int BrickDensity;
        public int UnbreakableDensity;
        public bool SameLevel;
    }

    public class GameIsStarted : ArkEvent { }

    public class GameIsOver : ArkEvent { }

    public class BallIsDamaged : ArkEvent { }

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
        private int _endlessDensity;
        private int _endlessUnbreakable;

        private int _life;

        private int _currentLevel;
        [SerializeField] private LevelLoader _levelLoader;
        private MainMenu _mainMenuHandler;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            _life = 5;
            Utils.Random = new System.Random();
            if (GameObject.Find("MainMenuHandler") != null)
            {
                _mainMenuHandler = GameObject.Find("MainMenuHandler").GetComponent<MainMenu>();
                GameMode = _mainMenuHandler.GameMode;
            }
            
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

                MessageBus.OnEvent<LevelIsCompleted>().Subscribe(evnt =>
                {
                    IDisposable d = null;
                    d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
                    {
                        GameIsStarted = false;
                        if (evnt.Succeed)
                        {
                            _currentLevel++;
                        }

                        if (_currentLevel < _questModeLevels.Count)
                        {
                            MessageBus.Publish(new GameIsInitiliazed()
                            {
                                BrickDensity = (int) _questModeLevels[_currentLevel].x,
                                UnbreakableDensity = (int) _questModeLevels[_currentLevel].y,
                                SameLevel = !evnt.Succeed
                            });
                        }
                        else
                        {
                            MessageBus.Publish(new GameIsOver());
                        }
                        d.Dispose();
                    });
                });

                UIManager.Instance.Init();
                _levelLoader.Init();

                MessageBus.OnEvent<GameIsOver>().Subscribe(evnt =>
                {
                    if (_mainMenuHandler != null)
                        Destroy(_mainMenuHandler.gameObject);
                    MessageBus.ClearAllEvents();
                    SceneManager.LoadScene("MainMenu");
                });

                MessageBus.Publish(new GameIsInitiliazed()
                {
                    BrickDensity = (int)_questModeLevels[_currentLevel].x,
                    UnbreakableDensity = (int)_questModeLevels[_currentLevel].y,
                    SameLevel = false
                });
            }

            else if (GameMode == GameMode.EndlessMode)
            {
                _currentLevel = 0;
                _endlessDensity = 20;
                _endlessUnbreakable = 0;

                BrickPools.InitiliazeBrickPools();

                MessageBus.OnEvent<LevelIsCompleted>().Subscribe(evnt =>
                {
                    IDisposable d = null;
                    d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
                    {
                        GameIsStarted = false;
                        if (evnt.Succeed)
                        {
                            _currentLevel++;
                            if(_endlessDensity < 100)
                                _endlessDensity++;
                            if(_endlessUnbreakable < 50)
                                _endlessUnbreakable++;
                        }

                        MessageBus.Publish(new GameIsInitiliazed()
                        {
                            BrickDensity = _endlessDensity,
                            UnbreakableDensity = _endlessUnbreakable,
                            SameLevel = !evnt.Succeed
                        });
                        d.Dispose();
                    });
                });

                UIManager.Instance.Init();
                _levelLoader.Init();

                MessageBus.OnEvent<GameIsOver>().Subscribe(evnt =>
                {
                    if (_mainMenuHandler != null)
                        Destroy(_mainMenuHandler.gameObject);
                    MessageBus.ClearAllEvents();
                    SceneManager.LoadScene("MainMenu");
                });

                MessageBus.Publish(new GameIsInitiliazed()
                {
                    BrickDensity = _endlessDensity,
                    UnbreakableDensity = _endlessUnbreakable,
                    SameLevel = false
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

        public int GetLife()
        {
            return _life;
        }

        public void BallIsDead()
        {
            _life--;
            MessageBus.Publish(new BallIsDamaged());

            if (_life <= 0)
            {
                MessageBus.Publish(new GameIsOver());
            }

            GameIsStarted = false;
        }
    }
}