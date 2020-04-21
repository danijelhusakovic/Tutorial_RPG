using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;

        private void Update() 
        {
            if(gameObject.tag.Equals("Player"))
            {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public float GetExperienceReward()
        {
            return 10f;
        }

        public int GetLevel()
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