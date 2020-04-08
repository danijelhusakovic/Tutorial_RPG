using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        GetComponent<NavMeshAgent>().destination = _target.position;
    }
}
