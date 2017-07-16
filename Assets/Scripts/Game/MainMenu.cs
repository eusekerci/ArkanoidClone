using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    public class MainMenu : MonoBehaviour
    {
        public GameMode GameMode;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void StartQuestMode()
        {
            GameMode = GameMode.QuestMode;
            SceneManager.LoadScene("GameScene");
        }

        public void StartInfiniteMode()
        {
            GameMode = GameMode.EndlessMode;
            SceneManager.LoadScene("GameScene");
        }
    }
}
