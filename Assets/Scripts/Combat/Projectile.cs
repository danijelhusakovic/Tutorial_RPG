using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;

    private Health _target = null;
    private float _damage = 0f;

    void Update()
    {
        if(_target == null) { return; }

        transform.LookAt(GetAimLocation());
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
        if(targetCapsule == null)
        {
            return _target.transform.position;
        }
        
        return _target.transform.position + Vector3.up * targetCapsule.height / 2f;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != _target) { return; }
        _target.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
