using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MinionScript : MonoBehaviour {
    [SerializeField] Transform position; //TODO: Add header properties
    [SerializeField] float speed;
    NavMeshAgent agent;

    void Start () {
        agent = GetComponent<NavMeshAgent>(); //TODO: Extract into function, RequestPositionFromLeader()
        agent.speed = speed;
	}

    void Update () {
        agent.SetDestination(position.position); //TODO: Check if near vicinity of position and switch to attack
	}
}
