using Game.Ammunitions;

using System;

using UnityEngine;

namespace Game
{
    [Serializable]
    public class Ammo : IAmmunition
    {
#pragma warning disable CS0649
        [SerializeField]
        private Ammunition type;
#pragma warning restore CS0649

        public int amount;

        public int TotalScore => amount * type.Score;

        public int ScorePerUnit => type.Score;

        public Sprite Sprite => type.Sprite;

        public void Shoot(Vector3 force, Vector3 position) => type.Shoot(force, position);

        public int GetPredictedPositions(Vector3 force, Vector3 startPosition, Vector2[] positions, float timeScale) => type.GetPredictedPositions(force, startPosition, positions, timeScale);
    }
}