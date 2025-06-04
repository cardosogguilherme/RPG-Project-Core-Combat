using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspiscionTime = 3f;

        private GameObject player;
        private Health health;
        private Fighter fighter;// => GetComponent<Fighter>();
        private Mover mover;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;


        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead) return;

            if (player == null) return;

            // print($"{gameObject.name} Distance from player: {distanceFromPlayer}");

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspiscionTime)
            {
                print($"{gameObject.name} Suspicious of player");
                SuspiscionBehavior();
            }
            else
            {
                GuardBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void SuspiscionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            // mover.Cancel();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private void GuardBehavior()
        {
            mover.StartMoveAction(guardPosition);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return chaseDistance > distanceToPlayer;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}