using Enderlook.Unity.Attributes;
using Enderlook.Unity.Components;
using Enderlook.Unity.Prefabs.FloatingText;
using Enderlook.Unity.Prefabs.HealthBarGUI;

using UnityEngine;

namespace Game.Creatures
{
    [RequireComponent(typeof(FloatingTextController)), RequireComponent(typeof(Hurtable)), AddComponentMenu("Game/Creatures/HealthBarManager")]
    public class HealthBarManager : MonoBehaviour, IDie
    {
#pragma warning disable CS0649
        [SerializeField]
        private HealthBar healthBar;

        [SerializeField, Tooltip("Determines in which point relative to this transfor does the healh bar spawns."), DrawVectorRelativeToTransform]
        private Vector3 healthBarSpawnPosition;
#pragma warning restore CS0649

        private FloatingTextController floatingTextController;

        private GameObject healthBarParent;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            floatingTextController = GetComponent<FloatingTextController>();

            Hurtable hurtable = GetComponent<Hurtable>();

            healthBarParent = new GameObject($"Enemy Health Bar Follower of {gameObject.name}");
            healthBar = Instantiate(healthBar, healthBarParent.transform);
            healthBar.ManualUpdate(hurtable.Health, hurtable.Health);

            hurtable.OnHealthChange += (health) => healthBar.UpdateValues(health);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Start()
        {
            healthBarParent.transform.position = transform.position + healthBarSpawnPosition;
            TransformFollower.AddTransformFollower(healthBarParent, transform,
                false, true, true, true,
                false, false, false, false,
                false, false, false);
        }

        void IDie.Die() => Destroy(healthBarParent);
    }
}
