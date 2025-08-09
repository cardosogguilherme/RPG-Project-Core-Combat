namespace RPG.SceneManagement
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Debug.Log("Transitioning to scene: " + sceneToLoad);
            Destroy(gameObject);
        }
    }
}