using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass _characterClass;
            [SerializeField] private float[] _health;
        }
    }
}