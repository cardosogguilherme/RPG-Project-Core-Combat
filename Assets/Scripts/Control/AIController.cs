using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private void Update()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            var distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            print($"{gameObject.name} Distance from player: {distanceFromPlayer}");
        }
    }
}