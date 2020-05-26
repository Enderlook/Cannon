using System;

using UnityEngine;
using UnityEngine.UI;

namespace Game.Creatures.Player.AbilitySystem
{
    [RequireComponent(typeof(Button))]
    public class AmmoUI : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        private Image ammoImage;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Text amountText;
#pragma warning restore CS0649

        private Action<int> selector;

        private int index;

        private Button button;

        public void SetSprite(Sprite sprite) => ammoImage.sprite = sprite;

        public void SetAmount(int amount)
        {
            amountText.text = amount.ToString();
            ammoImage.color = amount == 0 ? Color.gray : Color.white;
            button.interactable = amount > 0;
        }

        public void SetSelector(Action<int> selector, int index)
        {
            this.selector = selector;
            this.index = index;
            button = GetComponent<Button>();
            button.onClick.AddListener(Select);
        }

        public void Select()
        {
            selector(index);
            backgroundImage.color = Color.white;
        }

        public void UnSelect() => backgroundImage.color = Color.gray;
    }
}