using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float _fadeInTime = 0.2f;

        private const string DEFAULT_SAVE_FILE = "save";

        private void Awake() 
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() 
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            yield return fader.FadeIn(_fadeInTime);
        }

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

            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(DEFAULT_SAVE_FILE);
        }
    }
}