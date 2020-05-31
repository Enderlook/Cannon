using UnityEngine;

namespace Game.Ammunitions
{
    public class DestroyAfterCollision : MonoBehaviour
    {
        private float countdown;

        private void OnCollisionEnter2D(Collision2D collision)
            => Destroy(gameObject, countdown);

        public static void AddComponentTo(GameObject gameObject, float timeToDestroyAfterCollision)
        {
            DestroyAfterCollision component = gameObject.AddComponent<DestroyAfterCollision>();
            component.countdown = timeToDestroyAfterCollision;
        }
    }
}