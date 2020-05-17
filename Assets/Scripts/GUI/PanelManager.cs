using UnityEngine;

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
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        public void Update()
        {
            if (Input.GetKeyDown(closePanelsKey))
            {
                HidePanels();
                mainPanel.SetActive(true);
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
    }
}