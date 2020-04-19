using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon = null;
        [SerializeField] private float _respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.tag.Equals("Player")) { return; }

            other.GetComponent<Fighter>().EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            TogglePickup(false);
            yield return new WaitForSeconds(seconds);
            TogglePickup(true);
        }

        private void TogglePickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

    }
}
