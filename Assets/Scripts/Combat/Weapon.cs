using System;
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

        const string weaponName = "Weapon";


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab == null) return;
            if (animatorOverride == null) return;

            animator.runtimeAnimatorController = animatorOverride;

            var weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
            weapon.name = weaponName;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
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