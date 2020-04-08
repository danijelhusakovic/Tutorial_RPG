using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Ray _lastRay;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(_lastRay.origin, _lastRay.direction * 100f);

        GetComponent<NavMeshAgent>().destination = _target.position;
    }
}
