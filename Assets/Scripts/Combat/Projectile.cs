using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;
    [SerializeField] private bool _isHoming = true;

    private Health _target = null;
    private float _damage = 0f;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    void Update()
    {
        if (_target == null) { return; }

        if (_isHoming && !_target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return _target.transform.position;
        }

        return _target.transform.position + Vector3.up * targetCapsule.height / 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != _target) { return; }
        if(_target.IsDead()) { return; }
        _target.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
