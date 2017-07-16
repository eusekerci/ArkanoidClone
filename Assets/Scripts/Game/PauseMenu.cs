using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    public class PauseMenu : MonoBehaviour
    {
        public void ResumeButtonClick()
        {
            UIManager.Instance.TogglePauseMenu();
        }

        public void RestartButtonClick()
        {
            UIManager.Instance.TogglePauseMenu();
            MessageBus.Publish(new LevelIsCompleted() {Succeed = false});
        }

        public void MainMenuButtonClick()
        {
            UIManager.Instance.TogglePauseMenu();
            MessageBus.Publish(new GameIsOver());
        }

        public void NextLevelButtonClick()
        {
            UIManager.Instance.TogglePauseMenu();
            MessageBus.Publish(new LevelIsCompleted() {Succeed = true});
        }

        public void CheatToggleClick()
        {
            
        }
    }
}
