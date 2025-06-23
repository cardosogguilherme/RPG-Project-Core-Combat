using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private Boolean hasPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            print($"entered trigger {other.tag}");
            if (other.CompareTag("Player") && !hasPlayed)
            {
                hasPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}