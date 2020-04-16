using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasBeenTriggered = false;
        private void OnTriggerEnter(Collider other) 
        {
            if(_hasBeenTriggered) { return; }

            
            if(other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                _hasBeenTriggered = true;
            }
        }
    }
}