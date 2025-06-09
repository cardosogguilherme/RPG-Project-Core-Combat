using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
                Gizmos.DrawLine(
                    transform.GetChild(i).position,
                    transform.GetChild((i + 1) % transform.childCount).position);
            }
        }
    }
}
