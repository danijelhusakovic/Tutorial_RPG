using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;

        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, _startingLevel);
        }

        public float GetExperienceReward()
        {
            return 10f;
        }
    }
}