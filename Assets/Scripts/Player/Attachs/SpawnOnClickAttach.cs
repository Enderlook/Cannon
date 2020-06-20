using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Spawn On Click", menuName = "Game/Cannon/Attach/Spawn On Click")]
    public class SpawnOnClickAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Ammunitions spawn.")]
        private Ammunition[] ammunitions;

        [SerializeField, Tooltip("Amount randomization of force.")]
        private float randomization;

        [SerializeField, Tooltip("Amount of force inherited by pellets.")]
        private float inheritForceRatio = 1;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject) => SpawnOnClick.AddComponentTo(gameObject, ammunitions, randomization, inheritForceRatio);
    }
}
