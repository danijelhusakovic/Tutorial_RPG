using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private float _weaponDamage = 5f;

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
                // This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                _timeSinceLastAttack = 0f;
            }
        }

        // Animation Event
        void Hit()
        {
            Health healthComponent = _target.GetComponent<Health>();
            healthComponent.TakeDamage(_weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _weaponRange;
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