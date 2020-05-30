﻿using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Spawn On Hit", menuName = "Game/Cannon/Attach/Spawn On Hit")]
    public class SpawnOnHitAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Ammunitions spawn on hit.")]
        private Ammunition[] ammunitions;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject) => SpawnOnHit.AddComponentTo(gameObject, ammunitions);
    }
}