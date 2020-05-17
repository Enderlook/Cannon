using UnityEngine;
using UnityEngine.UI;

namespace Game.Creatures.Player.AbilitySystem
{
    public class AmmoUI : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        private Image ammoImage;

        [SerializeField]
        private Text amountText;
#pragma warning restore CS0649

        public void SetSprite(Sprite sprite) => ammoImage.sprite = sprite;

        public void SetAmount(int amount)
        {
            amountText.text = amount.ToString();
            ammoImage.color = amount == 0 ? Color.gray : Color.white;
        }
    }
}