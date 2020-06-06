using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GUI
{
    public class SceneChanger : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Manages panels shown in the menu.")]
        private PanelManager panelManager;

        [SerializeField, Tooltip("Manages loading bar.")]
        private Bar loadingBar;
#pragma warning restore CS0649


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
#pragma warning restore CS0649
        private void Awake() => Time.timeScale = 1;

        public void Exit() => Application.Quit();

        public void ChangeScene(int index)
        {
            panelManager.HideAll();
            loadingBar.Show();

            StartCoroutine(ShowProgess(SceneManager.LoadSceneAsync(index)));

            IEnumerator ShowProgess(AsyncOperation operation)
            {
                while (!operation.isDone)
                {
                    loadingBar.Percent(operation.progress);
                    yield return null;
                }
            }
        }

        public void Reload() => ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}