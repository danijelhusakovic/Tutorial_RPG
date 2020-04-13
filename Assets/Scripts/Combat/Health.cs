using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 100f;

        private bool _isDead = false;
        public bool IsDead() 
        {
            return _isDead;
            }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0f);
            if(_healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(_isDead) return;
            GetComponent<Animator>().SetTrigger("die");
            _isDead = true;
        }
    }
}