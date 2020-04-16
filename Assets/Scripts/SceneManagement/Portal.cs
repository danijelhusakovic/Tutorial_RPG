using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int _sceneToLoad = -1;

        private void OnTriggerEnter(Collider other) 
        {
            if(!other.tag.Equals("Player")) { return; }

            print("Portal triggered");
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}