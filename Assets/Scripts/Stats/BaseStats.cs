using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;
        [SerializeField] private GameObject _levelUpParticelEffect = null;
        [SerializeField] private bool _shouldUseModifiers = false;

        public event Action OnLevelUp;

        private LazyValue<int> _currentLevel;
        private Experience _experience;

        private void Awake() 
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() 
        {
            _currentLevel.ForceInit();
        }

        private void OnEnable() 
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained += UpdateLevel;
            }            
        }

        private void OnDisable() 
        {
            if(_experience != null)
            {
                _experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpParticelEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1f + GetPercentageModifiers(stat) / 100f);
        }


        private float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return _currentLevel.value;
        }

        private float GetAdditiveModifiers(Stat stat)
        {
            if(_shouldUseModifiers == false) { return 0f; }
            
            float total = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            if (_shouldUseModifiers == false) { return 0f; }

            float total = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(experience == null) { return _startingLevel; }

            float currentXP = experience.GetPoints();
            int penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, _characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if(xpToLevelUp >  currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}