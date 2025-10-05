using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;

        [field: SerializeField] public float WeaponRange { get; private set; } = 2f;
        [field: SerializeField] public float WeaponDamage { get; private set; } = 5f;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab == null) return;
            if (animatorOverride == null) return;

            animator.runtimeAnimatorController = animatorOverride;
            Instantiate(equippedPrefab, isRightHanded ? rightHand : leftHand);
        }
    }
}