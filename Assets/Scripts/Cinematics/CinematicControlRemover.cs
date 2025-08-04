namespace Rpg.Cinematics
{
    using UnityEngine;
    using RPG.Core;
    using UnityEngine.Playables;
    using RPG.Control;

    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;

        private void Start()
        {
            print("CinematicControlRemover started");

            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            print("Disabling control");

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("Enabling control");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}