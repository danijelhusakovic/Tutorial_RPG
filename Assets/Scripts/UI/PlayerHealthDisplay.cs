using RPG.Attributes;
using UnityEngine;

namespace RPG.UI
{

    public class PlayerHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _foreground;

        private Health _health;

        private void Start()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            float healthValue = Mathf.Clamp01(_health.GetFraction());
            _foreground.localScale = new Vector3(healthValue, _foreground.localScale.y, _foreground.localScale.z);
        }
    }

}