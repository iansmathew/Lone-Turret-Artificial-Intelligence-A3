using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour {

    [Header("General Properties")]
    float health = 100;
    

    [Header("Agent Properties")]
    [SerializeField] float speed = 4;

    Transform target;
    Transform player;

    //State Variables
    private State currentState;
    public AttackState attackState;
    public WalkState walkState;
    public DeathState deathState;

    //Component References
    Animator anim;
    NavMeshAgent agent;
    

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start () {
        anim.Play("RobotScaleUp");
        MoveToCoverPoint();

	}
	
	void Update () {
        currentState.Tick();
	}

    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if (currentState != null)
            currentState.OnStateEnter();
    }
}
