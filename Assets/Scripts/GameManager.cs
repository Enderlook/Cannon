using Enderlook.Unity.Components;

using Game.GUI;

using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private PanelManager panelManager;

        private int amount;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
#pragma warning restore CS0649
        private void Awake()
        {
            panelManager = FindObjectOfType<PanelManager>();
            Hurtable[] hurtables = FindObjectsOfType<Hurtable>();
            amount = hurtables.Length;

            foreach (Hurtable enemy in hurtables)
                DestroyNotifier.ExecuteOnDestroy(enemy.gameObject, DestroyOne);
        }

        private void DestroyOne()
        {
            if (--amount <= 0)
                panelManager.Win();
        }
    }
}