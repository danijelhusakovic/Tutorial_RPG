using System;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float _maxNavPathLength = 40f;

        private Health _health;

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
            if(InteractWithUI()) return;

            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            } 

            if(InteractWithComponent()) return;
            if(InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }


        private bool InteractWithUI()
        {
            bool isOverUI = EventSystem.current.IsPointerOverGameObject(); // It says GameObject, but it refers only to the UI.
            
            if(isOverUI) {SetCursor(CursorType.UI);}

            return isOverUI;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;

            bool hasHit = RaycastNavmesh(out target);

            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target);
                }
                SetCursor(CursorType.Move);
                return true;
            }
            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) { return false; }

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point,
                out navMeshHit, 
                 _maxNavMeshProjectionDistance, 
                 NavMesh.AllAreas);
            if(!hasCastToNavMesh) {return false;}

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            
            if (!hasPath) { return false; }

            if(path.status != NavMeshPathStatus.PathComplete) { return false; }
            if(GetPathLength(path) > _maxNavPathLength) { return false; }


            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;

            if(path.corners.Length < 2) { return total; } // 0

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                total += distance;
            }

            return total;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);


            return hits;
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