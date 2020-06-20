using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    [RequireComponent(typeof(AudioSource))]
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
        private AudioClip winClip;

        [SerializeField]
        private GameObject losePanel;

        [SerializeField]
        private AudioClip loseClip;

        [SerializeField]
        private AudioClip playClip;

        [SerializeField]
        private AudioClip menuClip;

        [SerializeField]
        private Score winScore;
#pragma warning restore CS0649

        private bool canChange = true;

        private AudioSource audioSource;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = toggleable ? playClip : menuClip;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        public void Update()
        {
            if (Input.GetKeyDown(closePanelsKey) && canChange)
            {
                HidePanels();
                if (Time.timeScale == 0)
                {
                    if (toggleable)
                    {
                        mainPanel.SetActive(false);
                        image.enabled = false;
                        Time.timeScale = 1;
                        Play(playClip);
                    }
                }
                else
                {
                    mainPanel.SetActive(true);
                    image.enabled = true;
                    Time.timeScale = 0;
                    Play(menuClip);
                }
            }

            if (!audioSource.isPlaying)
            {
                if (audioSource.clip == loseClip || audioSource.clip == winClip)
                    audioSource.clip = menuClip;
                audioSource.Play();
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
            Play(winClip);
            winScore.Value = FindObjectOfType<GameManager>().Score;
        }

        public void Lose()
        {
            OpenAndStuck();
            losePanel.SetActive(true);
            Play(loseClip);
        }

        private void Play(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}