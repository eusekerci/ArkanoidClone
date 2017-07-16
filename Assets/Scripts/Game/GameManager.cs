using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace Arkanoid
{
    public class GameIsInitiliazed : ArkEvent { }

    public class GameIsStarted : ArkEvent { }

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

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            BrickPools.InitiliazeBrickPools();
            MessageBus.Publish(new GameIsInitiliazed());

            MessageBus.OnEvent<LevelIsCompleted>().Subscribe(evnt =>
            {
                IDisposable d = null;
                d = Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(x =>
                {
                    GameIsStarted = false;
                    MessageBus.Publish(new GameIsInitiliazed());
                    d.Dispose();
                });
            });
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
    }
}