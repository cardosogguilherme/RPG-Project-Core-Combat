using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead) return;

            if (GetComponent<Health>().IsDead)
            {
                Cancel();
                return;
            }

            if (!IsTargetInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;

            // Trigger the attack animation Hit()
            TriggerAttack();
            timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("cancelAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;

            if (weapon == null) return;

            target.TakeDamage(weapon.WeaponDamage);
        }

        private bool IsTargetInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weapon.WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("cancelAttack");
            GetComponent<Animator>().ResetTrigger("attack");

            target = null;

            GetComponent<Mover>().Cancel();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health target = combatTarget.GetComponent<Health>();
            return target != null && !target.IsDead;
        }

    }
}