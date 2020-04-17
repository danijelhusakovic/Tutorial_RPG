using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public IEnumerator FadeOut(float duration)
        {
            while(_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime / duration;
                yield return null;  // Wait for one frame
            }
        }

        public IEnumerator FadeIn(float duration)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime / duration;
                yield return null;  // Wait for one frame
            }
        }
    }
}