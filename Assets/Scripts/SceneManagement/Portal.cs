using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;

        [SerializeField] private float _fadeOutDuration = 1f;
        [SerializeField] private float _fadeInDuration = 2f;
        [SerializeField] private float _fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) 
        {
            if(!other.tag.Equals("Player")) { return; }

            
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if(_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = GameObject.FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeOutDuration);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return fader.FadeIn(_fadeInDuration);

            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal._destination != _destination) { continue; }

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }
    }
}