using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    public class GameStartedEvent : ArkEvent { }

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

        public bool GameIsReady = false;

        void Start()
        {
            BrickPools.InitiliazeBrickPools();
            GameIsReady = true;
            MessageBus.Publish(new GameStartedEvent());
        }

        void Update()
        {
            //Testing
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}