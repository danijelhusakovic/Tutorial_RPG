using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _regenerationPercentage = 0.7f;
        [SerializeField] private TakeDamageEvent _takeDamage;
        [SerializeField] private UnityEvent _onDie;

        [System.Serializable ]
        public class TakeDamageEvent : UnityEvent<float> { }

        private LazyValue<float> _healthPoints;

        private bool _isDead = false;

        private void Awake() 
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() 
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable() 
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;   
        }

        private void OnDisable() 
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public bool IsDead() 
        {
            return _isDead;
            }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0f);

            if (_healthPoints.value == 0)
            {
                _onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                _takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100f * _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetFraction()
        {
            return _healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) { return; }

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if(_isDead) return;

            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<Animator>().SetTrigger("die");
            
            _isDead = true;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * _regenerationPercentage;
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float) state;

            if (_healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}