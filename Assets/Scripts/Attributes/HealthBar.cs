using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health = null;
        [SerializeField] private RectTransform _foreground = null;
        [SerializeField] private Canvas _canvas = null;

        private void Update() 
        {
            if (Mathf.Approximately(_health.GetFraction(), 0f)
            ||  Mathf.Approximately(_health.GetFraction(), 1f))
            {
                _canvas.enabled = false;
                return;
            }

            _canvas.enabled = true;
            _foreground.localScale = new Vector3(_health.GetFraction(), 1f, 1f);
        }
    }

}