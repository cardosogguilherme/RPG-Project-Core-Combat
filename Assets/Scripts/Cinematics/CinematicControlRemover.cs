namespace Rpg.Cinematics
{
    using UnityEngine;
    using RPG.Core;
    using UnityEngine.Playables;

    public class CinematicControlRemover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void DisableControl(PlayableDirector playableDirector)
        {
            print("Disabling control");
        }

        void EnableControl(PlayableDirector playableDirector)
        {
            print("Enabling control");
        }
    }
}