using UnityEngine;

namespace Game.Ammunitions
{
    public interface IAmmunition
    {
        Sprite Sprite { get; }

        void Shoot(Vector3 force, Vector3 position);

        int GetPredictedPositions(Vector3 force, Vector3 startPosition, Vector2[] positions, float timeScale);
    }
}
