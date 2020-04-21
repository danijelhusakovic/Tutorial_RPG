using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints = 0f;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
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