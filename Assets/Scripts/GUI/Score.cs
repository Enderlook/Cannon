using System;

using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(Text))]
    public class Score : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        private float showSpeedFlat;

        [SerializeField, Min(0)]
        private float showSpeedPercent;
#pragma warning restore CS0649

        private Text text;

        [NonSerialized]
        public int Value;

        private Color defaultColor;

        public int ValueWithoutEffect {
            get => Value;
            set {
                this.value += value - Value;
                Value = value;
            }
        }

        private float value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            text = GetComponent<Text>();
            defaultColor = text.color;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            Color color = text.color;
            if (value < Value)
                color = Color.Lerp((defaultColor + Color.green) / 2, Color.green, Mathf.PingPong(Time.realtimeSinceStartup * 3, 1));
            else if (value > Value)
                color = Color.Lerp((defaultColor + Color.red) / 2, Color.red, Mathf.PingPong(Time.realtimeSinceStartup * 3, 1));
            else if (color != defaultColor)
                color = new Color(MoveTowards(color.r, defaultColor.r), MoveTowards(color.g, defaultColor.g), MoveTowards(color.b, defaultColor.b), defaultColor.a);
            color.a = defaultColor.a;

            value = Mathf.Lerp(value, Value, showSpeedPercent * Time.unscaledDeltaTime);
            value = Mathf.MoveTowards(value, Value, showSpeedFlat * Time.unscaledDeltaTime);
            text.text = ((int)value).ToString();
            text.color = color;

            float MoveTowards(float a, float b) => Mathf.MoveTowards(a, b, Time.unscaledDeltaTime);
        }
    }
}