using System;
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

        public event Action OnLevelUp;

        private int _currentLevel = 0;

        private void Start() 
        {
            _currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if(newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
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
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
        }

        public int CalculateLevel()
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