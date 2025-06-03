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

        private GameObject player;
        private Health health;
        private Fighter fighter;// => GetComponent<Fighter>();
        private Mover mover;
        Vector3 guardPosition;


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
                ChasePlayer();
            }
            else
            {
                StopChase();
            }
        }

        private void ChasePlayer()
        {
            fighter.Attack(player);
        }

        private void StopChase()
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