using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        [field: SerializeField] public float WeaponRange { get; private set; } = 2f;
        [field: SerializeField] public float WeaponDamage { get; private set; } = 5f;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab == null) return;
            if (animatorOverride == null) return;

            animator.runtimeAnimatorController = animatorOverride;
            Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(
                projectile,
                GetHandTransform(
                    rightHand,
                    leftHand
                ).position,
                Quaternion.identity
            );

            projectileInstance.SetTarget(target, WeaponDamage);
        }
    }
}