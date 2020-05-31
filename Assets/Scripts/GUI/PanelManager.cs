using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class PanelManager : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Managed panels.")]
        private GameObject[] panels;

        [SerializeField, Tooltip("Main panel.")]
        private GameObject mainPanel;

        [SerializeField, Tooltip("Key pressed to close panels.")]
        private KeyCode closePanelsKey;

        [SerializeField, Tooltip("Frame image of menu.")]
        private Image image;

        [SerializeField, Tooltip("Whenever this menu can be hidden or not.")]
        private bool toggleable;

        [SerializeField]
        private GameObject winPanel;

        [SerializeField]
        private GameObject losePanel;
#pragma warning restore CS0649

        private bool canChange = true;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        public void Update()
        {
            if (Input.GetKeyDown(closePanelsKey) && canChange)
            {
                HidePanels();
                if (mainPanel.activeSelf)
                {
                    if (toggleable)
                    {
                        mainPanel.SetActive(false);
                        image.enabled = false;
                        Time.timeScale = 0;
                    }
                }
                else
                {
                    mainPanel.SetActive(true);
                    image.enabled = true;
                    Time.timeScale = 1;
                }
            }
        }

        private void HidePanels()
        {
            for (int i = 0; i < panels.Length; i++)
                panels[i].SetActive(false);
        }

        public void HideAll()
        {
            HidePanels();
            mainPanel.SetActive(false);
        }

        private void OpenAndStuck()
        {
            image.enabled = true;
            canChange = false;
            Time.timeScale = 0;
        }

        public void Win()
        {
            OpenAndStuck();
            winPanel.SetActive(true);
        }

        public void Lose()
        {
            OpenAndStuck();
            losePanel.SetActive(true);
        }
    }
}