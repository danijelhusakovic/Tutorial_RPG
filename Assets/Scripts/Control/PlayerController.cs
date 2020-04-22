using System;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;

        public enum CursorType
        {
            None,
            Move,
            Attack,
            Pickup
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType Type;
            public Vector2 Hotspot;
            public Texture2D Texture;
        }

        [SerializeField] private CursorMapping[] _cursorMappings = null;


        private void Awake() 
        {
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;

            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;

                if(!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                SetCursor(CursorType.Attack);
                return true;
            }
            return false;
        }


        private bool InteractWithMovement()
        {
            RaycastHit hit;

            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                SetCursor(CursorType.Move);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.Texture, mapping.Hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if(mapping.Type.Equals(type)) { return mapping; }
            }

            return _cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}