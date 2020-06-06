using Enderlook.Unity.Attributes;

using Game.Creatures.Player.AbilitySystem;
using Game.GUI;

using System.Linq;

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

        [SerializeField, Tooltip("Layer used by projectiles."), Layer]
        private int layer;

        [Header("Prediction")]
        [SerializeField, Min(.01f), Tooltip("Time scale used to draw projectile predictions.")]
        private float predictionTimeScale = .1f;

        [SerializeField, Min(2), Tooltip("Max amount of predicted points.")]
        private int predictedPointsAmount = 25;

        [SerializeField, Tooltip("Sprite used to draw dots.")]
        private Sprite dotSprite;

        [SerializeField, Min(1), Tooltip("Each how much points an sprite must be draw.")]
        private int spriteRatio = 1;

        [SerializeField, Tooltip("Size of sprites.")]
        private float spriteScale = 1;

        [Header("UI")]
        [SerializeField, Tooltip("Rect Transform where UI elements of ammunition are placed.")]
        private RectTransform uiTransform;

        [SerializeField, Tooltip("Prefab used to for ui elements of ammunition.")]
        private AmmoUI uiPrefab;

        [Header("Setup")]
        [SerializeField, Tooltip("Sound play on shoot.")]
        private AudioClip shootSound;

        [SerializeField, Tooltip("Audio source used to play sounds.")]
        private AudioSource audioSource;
#pragma warning restore CS0649

        private int currentAmmunitionIndex;

        private Vector2[] predictedPositions;

        private (Transform, SpriteRenderer)[] dots;

        private AmmoUI[] uis;

        private LineRenderer lineRenderer;

        private bool HasAmmo => ammunitions.Any(e => e.amount > 0);

        private float loseCountDown;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            uis = new AmmoUI[ammunitions.Length];
            for (int i = 0; i < uis.Length; i++)
            {
                AmmoUI instance = Instantiate(uiPrefab, uiTransform);
                uis[i] = instance;
                Ammo ammo = ammunitions[i];
                instance.SetSprite(ammo.Sprite);
                instance.SetSelector(SelectAmmunition, i);
                instance.SetAmount(ammo.amount);
                instance.UnSelect();
            }
            uis[0].Select();

            #region Gizmos
            predictedPositions = new Vector2[predictedPointsAmount];
            dots = new (Transform, SpriteRenderer)[predictedPointsAmount / spriteRatio];
            Transform parent = new GameObject("Dots").transform;
            for (int i = 0; i < dots.Length; i++)
            {
                GameObject dot = new GameObject($"Dot #{i}");
                SpriteRenderer spriteRenderer = dot.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = dotSprite;
                Transform dotTransform = dot.transform;
                dotTransform.parent = parent;
                dotTransform.localScale = Vector3.one * spriteScale;
                dots[i] = (dotTransform, spriteRenderer);
            }

            lineRenderer = GetComponent<LineRenderer>();
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(new Keyframe(0, 0));
            curve.AddKey(new Keyframe(.1f, 1));
            curve.AddKey(new Keyframe(.9f, 1));
            curve.AddKey(new Keyframe(1, 0));
            lineRenderer.widthCurve = curve;
            #endregion
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Update()
        {
            Vector2 mousePosition = GetMousePosition();
            Vector2 mouseDirection = GetMouseDirection(mousePosition);
            Vector2 shootingPosition = GetShootingPosition(mouseDirection);
            Vector2 shootingForce = GetShootingForce(mouseDirection);

            if (IsInShootingRange(mousePosition))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ammo ammunition = ammunitions[currentAmmunitionIndex];
                    if (ammunition.amount > 0)
                    {
                        uis[currentAmmunitionIndex].SetAmount(--ammunition.amount);
                        ammunition.Shoot(shootingForce, shootingPosition);

                        audioSource.pitch = Random.Range(.8f, 1.2f);
                        audioSource.PlayOneShot(shootSound, Random.Range(.8f, 1));
                    }

                    if (!HasAmmo)
                        loseCountDown = 3;
                }

                #region Gizmos
                int max = ammunitions[currentAmmunitionIndex].GetPredictedPositions(GetShootingForce(mouseDirection), shootingPosition, predictedPositions, predictionTimeScale);
                if (max > 0)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.positionCount = max;
                    lineRenderer.SetPosition(0, shootingPosition);

                    int i = 1;
                    for (; i < max; i++)
                    {
                        int j = i - 1;
                        Vector2 position = predictedPositions[j];
                        lineRenderer.SetPosition(i, position);
                        if (j % spriteRatio == 0)
                        {
                            (Transform, SpriteRenderer) dot = dots[j / spriteRatio];
                            dot.Item1.position = position;
                            dot.Item2.enabled = true;
                        }
                    }

                    for (--i; i < dots.Length; i++)
                        if (i % spriteRatio == 0)
                            dots[i / spriteRatio].Item2.enabled = false;
                }
                else
                    Disable();
                #endregion
            }
            else
                #region Gizmos
                Disable();

            void Disable()
            {
                lineRenderer.enabled = false;
                for (int i = 0; i < dots.Length; i++)
                    if (i % spriteRatio == 0)
                        dots[i / spriteRatio].Item2.enabled = false;
            }
            #endregion Gizmos

            if (loseCountDown > 0)
            {
                if (HasAmmo)
                    loseCountDown = 0;
                else
                {
                    loseCountDown -= Time.deltaTime;
                    if (loseCountDown <= 0)
                        FindObjectOfType<PanelManager>().Lose();
                }
            }
        }

        private bool IsInShootingRange(Vector2 mousePosition) => Vector3.Distance(mousePosition, center) <= maximumShootingDistance * 2;

        private Vector2 GetShootingPosition(Vector2 mouseDirection)
            => (mouseDirection.normalized * maximumShootingDistance) + center;

        private Vector2 GetMouseDirection(Vector2 mousePosition) => Vector2.ClampMagnitude(center - mousePosition, maximumShootingDistance);

        private static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        private Vector2 GetShootingForce(Vector2 mouseDirection)
            => mouseDirection * (maximumShootingDistance / mouseDirection.magnitude) * maximumForce;

        private void SelectAmmunition(int index)
        {
            uis[currentAmmunitionIndex].UnSelect();
            currentAmmunitionIndex = index;
        }

        #region Gizmos
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(center, maximumShootingDistance);

            if (Application.isPlaying)
            {
                Vector2 mousePosition = GetMousePosition();
                if (IsInShootingRange(mousePosition))
                {
                    Vector2 mouseDirection = GetMouseDirection(mousePosition);
                    Vector2 shootingPosition = GetShootingPosition(mouseDirection);

                    Gizmos.DrawLine(center, center - mouseDirection);
                    Gizmos.DrawLine(center, center + ((shootingPosition - center).normalized * shootingDistanceFromCenter));
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
        #endregion
    }
}