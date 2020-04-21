using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints = 0f;

        public event Action OnExperienceGained;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float) state;
        }

        public float GetPoints()
        {
            return _experiencePoints;
        }
    }
}