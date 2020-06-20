﻿using Enderlook.Unity.Components;

using Game.GUI;

using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Score score;

        private PanelManager panelManager;

        private int amount;

        public int Score => score.Value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            panelManager = FindObjectOfType<PanelManager>();
            Creature[] creatures = FindObjectsOfType<Creature>();
            amount = creatures.Length;

            foreach (Creature enemy in creatures)
                DestroyNotifier.ExecuteOnDestroy(enemy.gameObject, DestroyOne);

            foreach (Hurtable hurtable in FindObjectsOfType<Hurtable>())
                DestroyNotifier.ExecuteOnDestroy(hurtable.gameObject, IncreaseScore);
        }

        private void DestroyOne()
        {
            if (--amount <= 0)
                panelManager.Win();
        }

        private void IncreaseScore(GameObject gameObject)
            => score.Value += gameObject.GetComponent<Hurtable>().Score;
    }
}