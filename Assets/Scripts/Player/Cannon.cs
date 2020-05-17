using Enderlook.Unity.Attributes;

using Game.Creatures.Player.AbilitySystem;

using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(LineRenderer))]
    public class Cannon : MonoBehaviour
    {
#pragma warning disable CS0649
        [Header("Shooting")]
        [SerializeField, Tooltip("Center used to calculate force and direction."), DrawVectorRelativeToTransform]
        private Vector2 center;

        [SerializeField, Tooltip("Shooting distance point from center.")]
        private float shootingDistanceFromCenter = 1;

        [SerializeField, Tooltip("Maximum shooting distance from the center.")]
        private float maximumShootingDistance = 1;

        [SerializeField, Min(.1f), Tooltip("Force applied at maximum distance from center.")]
        private float maximumForce = 1;

        [SerializeField, Tooltip("Ammunitions of player and ammmount.")]
        private Ammo[] ammunitions;

        [Header("Prediction")]
        [SerializeField, Min(.01f), Tooltip("Time scale used to draw projectile predictions.")]
        private float predictionTimeScale = .1f;

        [SerializeField, Min(2), Tooltip("Max amount of predicted points.")]
        private int predictedPointsAmount = 25;

        [SerializeField, Min(.1f), Tooltip("Width multiplier of the line renderer.")]
        private float widthMultiplier = 1;

        [Header("UI")]
        [SerializeField, Tooltip("Rect Transform where UI elements of ammunition are placed.")]
        private RectTransform uiTransform;

        [SerializeField, Tooltip("Prefab used to for ui elements of ammunition.")]
        private AmmoUI uiPrefab;
#pragma warning restore CS0649

        private int currentAmmunitionIndex;

        private Vector2[] predictedPositions;

        private float[] predictedSpeeds;

        private AmmoUI[] uis;

        private LineRenderer lineRenderer;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            predictedPositions = new Vector2[predictedPointsAmount];
            predictedSpeeds = new float[predictedPointsAmount];

            uis = new AmmoUI[ammunitions.Length];
            for (int i = 0; i < uis.Length; i++)
            {
                AmmoUI instance = Instantiate(uiPrefab, uiTransform);
                uis[i] = instance;
                Ammo ammo = ammunitions[i];
                instance.SetSprite(ammo.Sprite);
                instance.SetAmount(ammo.amount);
            }

            lineRenderer = GetComponent<LineRenderer>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            Vector2 mouseDirection = GetMouseDirection();
            Vector2 shootingPosition = GetShootingPosition(mouseDirection);
            Vector2 shootingForce = GetShootingForce(mouseDirection);

            if (Input.GetMouseButtonDown(0))
            {
                Ammo ammunition = ammunitions[currentAmmunitionIndex];
                if (ammunition.amount > 0)
                {
                    uis[currentAmmunitionIndex].SetAmount(--ammunition.amount);
                    ammunition.Shoot(shootingForce, shootingPosition);
                }
            }

            int max = ammunitions[currentAmmunitionIndex].GetPredictedPositions(GetShootingForce(mouseDirection), shootingPosition, predictedPositions, predictionTimeScale);
            if (max > 0)
            {
                lineRenderer.positionCount = max;
                lineRenderer.SetPosition(0, shootingPosition);

                predictedSpeeds[0] = (predictedPositions[0] - shootingPosition).magnitude;

                for (int i = 1; i < max; i++)
                {
                    Vector2 position = predictedPositions[i - 1];
                    predictedSpeeds[i] = (position - predictedPositions[i]).magnitude;
                    lineRenderer.SetPosition(i, position);
                }

                float fastest = float.MinValue;
                for (int i = 0; i < max; i++)
                {
                    if (predictedSpeeds[i] > fastest)
                        fastest = predictedSpeeds[i];
                }

                AnimationCurve curve = new AnimationCurve();
                for (int i = 0; i < max; i++)
                {
                    float time = (float)i / max;
                    float percent = predictedSpeeds[i] / fastest;
                    curve.AddKey(time, percent);
                }
                lineRenderer.widthCurve = curve;
                lineRenderer.widthMultiplier = widthMultiplier;
            }
        }

        private Vector2 GetShootingPosition(Vector2 mouseDirection)
            => mouseDirection + center;

        private Vector2 GetMouseDirection() => Vector2.ClampMagnitude((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - center, maximumShootingDistance);

        private Vector2 GetShootingForce(Vector2 mouseDirection)
            => mouseDirection * (maximumShootingDistance / mouseDirection.magnitude) * maximumForce;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(center, maximumShootingDistance);

            if (Application.isPlaying)
            {
                Vector2 mouseDirection = GetMouseDirection();
                Vector2 shootingPosition = GetShootingPosition(mouseDirection);

                Gizmos.DrawLine(shootingPosition, center);
                Gizmos.DrawLine(center, center + ((center - shootingPosition).normalized * shootingDistanceFromCenter));
                Gizmos.DrawWireSphere(shootingPosition, .1f);

                int max = ammunitions[currentAmmunitionIndex].GetPredictedPositions(GetShootingForce(mouseDirection), shootingPosition, predictedPositions, predictionTimeScale);
                if (max > 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(predictedPositions[0], .1f);
                    for (int i = 0; i < max - 1;)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(predictedPositions[i], predictedPositions[++i]);
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(predictedPositions[i], .025f);
                    }
                }
            }
        }
    }
}