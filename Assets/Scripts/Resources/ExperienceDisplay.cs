using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;

        private void Awake() 
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() 
        {
            GetComponent<Text>().text = String.Format("{0:0}", _experience.GetPoints());
        }
    }
}