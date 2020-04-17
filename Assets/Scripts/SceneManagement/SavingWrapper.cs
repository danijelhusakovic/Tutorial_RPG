using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DEFAULT_SAVE_FILE = "save";

        private void Update() 
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void Save()
        {
            GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
        }

        private void Load()
        {
            GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }
    }
}