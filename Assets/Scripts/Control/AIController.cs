using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private float _suspicionTime = 3f;

        private GameObject _player;
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;

        private Vector3 _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start() 
        {
            _player = GameObject.FindWithTag("Player");
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if(_health.IsDead()) return;

            if(InAttackRangeOfPlayer() && _fighter.CanAttack(_player))
            {
                _timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if(_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                GuardBehaviour();
            }

            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            _mover.StartMoveAction(_guardPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < _chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}