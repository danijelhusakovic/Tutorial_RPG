using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistentObjectPrefab;

        private static bool _hasSpawned = false;

        private void Awake()
        {
            if(_hasSpawned) { return; }
            _hasSpawned = true;

            SpawnPersistentObjects();
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}