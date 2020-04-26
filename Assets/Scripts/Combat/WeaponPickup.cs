using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weapon = null;
        [SerializeField] private float _healthToRestore = 0f;
        [SerializeField] private float _respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.tag.Equals("Player")) { return; }

            Pickup(other.gameObject);
        }

        private void Pickup(GameObject subject)
        {
            if(_weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weapon);
            }

            if(_healthToRestore > 0f)
            {
                subject.GetComponent<Health>().Heal(_healthToRestore);
            }
            
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

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
