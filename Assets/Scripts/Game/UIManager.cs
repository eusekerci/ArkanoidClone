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
        public Text Life;
        public PauseMenu PauseMenu;
        public bool PauseMenuActive;

        void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            MessageBus.OnEvent<LevelIsLoaded>().Subscribe(evnt =>
            {
                LevelName.text = "Level: " + (GameManager.Instance.GetLevelIndex()+1).ToString();
                LevelDifficulty.text = "Difficulty: " + (evnt.Complexity / 100).ToString();
            });
            MessageBus.OnEvent<BallIsDamaged>().Subscribe(evnt =>
            {
                Life.text = "";
                for(int i=0; i<GameManager.Instance.GetLife(); i++)
                    Life.text += "♥";
            });

            PauseMenuActive = false;
            PauseMenu.gameObject.SetActive(PauseMenuActive);
        }

        public void TogglePauseMenu()
        {
            PauseMenuActive = !PauseMenuActive;
            PauseMenu.gameObject.SetActive(PauseMenuActive);
            Time.timeScale = PauseMenuActive ? 0 : 1;
        }
    }
}
