using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Ammunitions
{
    public class SpawnOnClick : MonoBehaviour
    {
        private Ammunition[] ammunitions;

        private float randomization;

        private float inheritForceRatio;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            if (!Input.GetMouseButtonDown(1))
                return;

            if (ammunitions.Length == 0)
                return;

            Ammunition ammunition = ammunitions[0];
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            ammunition.Shoot(rigidbody.velocity * rigidbody.mass * inheritForceRatio, transform.position);

            for (int i = 0; i < ammunitions.Length - 1; i++)
            {
                Vector2 random = (Vector2.one * (1 - randomization)) + ((Vector2)Random.onUnitSphere * randomization);
                ammunitions[i].Shoot(rigidbody.velocity * rigidbody.mass * inheritForceRatio * random, transform.position);
            }
            Destroy(gameObject);
        }

        public static void AddComponentTo(GameObject gameObject, Ammunition[] ammunitions, float randomization, float inheritForceRatio)
        {
            SpawnOnClick component = gameObject.AddComponent<SpawnOnClick>();
            component.ammunitions = ammunitions;
            component.randomization = randomization;
            component.inheritForceRatio = inheritForceRatio;
        }
    }
}