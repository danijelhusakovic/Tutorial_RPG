using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttacks = 1f;

        private Transform _target;
        private float _timeSinceLastAttack;

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if(_target == null) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(_timeSinceLastAttack > _timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                _timeSinceLastAttack = 0f;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
        }

        // Animation Event
        void Hit()
        {

        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }
    }
}