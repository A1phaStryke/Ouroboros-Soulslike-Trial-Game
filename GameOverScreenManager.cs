using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace AP
{
    public class GameOverScreenManager : MonoBehaviour
    {
        public static GameOverScreenManager instance;

        [Header("Fade In")]
        [SerializeField] CanvasGroup gameOverCanvasGroup; // Allows us to set the alpha to fade over time

        int mainMenuSceneIndex = 0;
        int worldSceneIndex = 1;

        float restartTimer = 0.5f;

        bool restartPressed = false;
        bool restartButtonClicked = false;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnEnable()
        {
            StartCoroutine(FadeInGameOverScreenOverTime(gameOverCanvasGroup, 5));
        }

        public void Update()
        {
            if (restartButtonClicked)
            {
                
                if (restartTimer > 0)
                {
                    TimerDecrease();

                    if (restartPressed == true)
                    {
                        RestartGameInitial();
                        restartPressed = false;
                    }
                }
                else
                {
                    StartCoroutine(LoadRestartedGame());
                }
            }
        }

        public void RestartButtonClicked()
        {
            restartButtonClicked = true;
            restartPressed = true;
        }

        public void RestartGameInitial()
        {
            NetworkManager.Singleton.Shutdown();
        }

        public IEnumerator LoadRestartedGame()
        {
            Debug.Log("GAME RESTART");
            NetworkManager.Singleton.StartHost();
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
            AsyncOperation loadWorldOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }

        public void GoToMainMenu()
        {
            StartCoroutine(LoadMainMenuScene());
        }

        public IEnumerator LoadMainMenuScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(mainMenuSceneIndex);
            NetworkManager.Singleton.Shutdown();

            yield return null;
        }

        public int GetMainMenuSceneIndex()
        {
            return mainMenuSceneIndex;
        }

        private IEnumerator FadeInGameOverScreenOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;

            yield return null;
        }

        private void TimerDecrease()
        {
            Debug.Log("Timer decrease");
            restartTimer -= Time.deltaTime;
        }
    }
}