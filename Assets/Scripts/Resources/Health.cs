using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPoints = 100f;

        private bool _isDead = false;

        private void Start() 
        {
            _healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead() 
        {
            return _isDead;
            }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0f);
            CheckIfDead();
        }

        public float GetPercentage()
        {
            return 100f * _healthPoints / GetComponent<BaseStats>().GetHealth();
        }

        private void CheckIfDead()
        {
            if (_healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(_isDead) return;

            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("die");
            
            _isDead = true;
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float) state;
            
            CheckIfDead();
        }
    }
}