using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class Bar : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Frame image of bar.")]
        private Image frame;

        [SerializeField, Tooltip("Content image of bar.")]
        private Image fill;

        [SerializeField, Tooltip("Text value of bar.")]
        private Text text;
#pragma warning restore CS0649

        public void Show()
        {
            frame.gameObject.SetActive(true);
            fill.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
        }

        public void Percent(float value)
        {
            fill.fillAmount = value;
            text.text = Mathf.FloorToInt(value * 100) + "%";
        }
    }
}