using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Trail", menuName = "Game/Cannon/Attach/Trail")]
    public class TrailAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Trail renderer material")]
        private Material trailMaterial;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject)
        {
            TrailRenderer trailRenderer = gameObject.AddComponent<TrailRenderer>();
            trailRenderer.material = trailMaterial;
            trailRenderer.widthMultiplier = .1f;
            trailRenderer.startWidth = .25f;
            trailRenderer.endWidth = 0;
            trailRenderer.time = 1f;
        }
    }
}
