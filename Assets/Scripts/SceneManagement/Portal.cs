namespace RPG.SceneManagement
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}