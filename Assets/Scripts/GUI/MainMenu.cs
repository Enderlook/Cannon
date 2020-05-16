using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GUI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("Index of the scene to play.")]
        private int startLevelScene;

        [SerializeField, Tooltip("Manages panels shown in the menu.")]
        private PanelManager panelManager;

        [SerializeField, Tooltip("Manages loading bar.")]
        private Bar loadingBar;

        public void Exit() => Application.Quit();

        public void Play()
        {
            panelManager.HideAll();
            loadingBar.Show();

            StartCoroutine(ShowProgess(SceneManager.LoadSceneAsync(startLevelScene)));

            IEnumerator ShowProgess(AsyncOperation operation)
            {
                while (!operation.isDone)
                {
                    loadingBar.Percent(operation.progress);
                    yield return null;
                }
            }
        }
    }
}