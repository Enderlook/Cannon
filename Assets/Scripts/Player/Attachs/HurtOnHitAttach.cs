using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Hurt On Hit", menuName = "Game/Cannon/Attach/Hurt On Hit")]
    public class HurtOnHitAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Damage multiplied done.")]
        private float damageMultiplier = 1;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject) => HurtOnHit.AddComponentTo(gameObject, damageMultiplier);
    }
}
