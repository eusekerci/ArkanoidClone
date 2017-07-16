using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{

    public class UIManager : MonoBehaviour
    {
        #region Singleton

        private static UIManager instance;

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        #endregion

        public Text LevelName;
        public Text LevelDifficulty;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            MessageBus.OnEvent<LevelIsLoaded>().Subscribe(evnt =>
            {
                LevelName.text = "Level: " + (GameManager.Instance.GetLevelIndex()+1).ToString();
                LevelDifficulty.text = "Difficulty: " + (evnt.Complexity / 100).ToString();
            });
        }
    }
}
