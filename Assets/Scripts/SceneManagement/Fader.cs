using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private Coroutine _currentlyActiveFade = null;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1f;
        }

        public Coroutine FadeOut(float duration)
        {
            return Fade(1f, duration);
        }

        public Coroutine FadeIn(float duration)
        {
            return Fade(0f, duration);
        }

        public Coroutine Fade(float target, float duration)
        {
            if (_currentlyActiveFade != null) { StopCoroutine(_currentlyActiveFade); }

            _currentlyActiveFade = StartCoroutine(FadeRoutine(target, duration));
            return _currentlyActiveFade;
        } 

        private IEnumerator FadeRoutine(float target, float duration)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / duration);
                yield return null;  // Wait for one frame
            }
        }

    }
}