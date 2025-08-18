namespace RPG.SceneManagement
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.SceneManagement;

    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);

            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }



        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                Debug.Log($"Found portal: {portal.name} with destination {portal.destination}");
                Debug.Log($"Portal position: {portal.transform.position}");
                return portal;
            }

            return null;
        }
    }
}