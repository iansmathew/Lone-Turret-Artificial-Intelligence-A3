using UnityEngine;
using UnityEngine.AI;
using cakeslice;

public class BossScript : MonoBehaviour {

    [Header("General Properties")]
    float health = 100;

    [Header("Agent Properties")]
    [SerializeField] float speed = 4;

    public Vector3 target;
    Transform player;

    //State Variables
    private State<BossScript> currentState;
    public BossAttackState attackState;
    public BossWalkState walkState;
    public BossDefendState defendState;
    bool inAttackState = false;
    bool inDefendState = false;
    bool towardsPlayer = true;
    bool hasUsedDefend = false;
    float timeToSwitchToDefendState = 5;
    bool firstHit = false;
    float lastHit;

    [Header("On Select Properties")]
    Outline outlineScript;
    bool mouseIsOver = false;

    //Component References
    Animator anim;
    NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        outlineScript = GetComponentInChildren<Outline>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackState = new BossAttackState(this);
        walkState = new BossWalkState(this);
        defendState = new BossDefendState(this);

        SetState(walkState);
    }

    void Start () {
        anim.Play("RobotScaleUp");
	}
	
	void Update () {
        currentState.Tick();
        if (mouseIsOver)
            outlineScript.color = 1;
        else if (EnemyManagerScript.Instance.selectedEnemy == this.gameObject)
            outlineScript.color = 2;
        else
            outlineScript.color = 0;
    }

    private void OnMouseEnter()
    {
        mouseIsOver = true;
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!inDefendState && collision.gameObject.tag == "pBullet") //dont take damage in defend state
        {
            if (!inAttackState && !firstHit)
            {
                firstHit = true; //switch flag so this log doesnt happen again
                lastHit = Time.time; //log time when hit occured
            }
            TakeHit(0.2f);
        }
    }

    private void TakeHit(float _damage)
    {
        anim.SetTrigger("Take Damage");
        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetState(State<BossScript> state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    public void EnterWalk()
    {
        anim.SetBool("Move Forward", true);
        agent.isStopped = false;

        target = towardsPlayer ? player.transform.position : EnemyManagerScript.Instance.RequestCoverPoint();
        towardsPlayer = !towardsPlayer; //Toggle between player and cover point 
        agent.SetDestination(target);
    }

    public void ExitWalk()
    {
        anim.SetBool("Move Forward", false);
    }

    public void CheckIfReachTarget()
    {
        if (!agent.pathPending)
        {
            if (!agent.hasPath)
            //Clean up and ready to switch to attack state
            {
                agent.isStopped = true;
                SetState(attackState);
            }
        }
    }

    public void EnterAttack()
    {
        inAttackState = true;
        Invoke("SwitchBackToWalk", !towardsPlayer ? Random.Range(5.0f, 7.0f) : Random.Range(3.0f, 5.0f));
    }

    public void ExitAttack()
    {
        inAttackState = false;
        anim.SetBool("Both Rapid Attack", false);
        anim.SetBool("Spin Attack", false);

        hasUsedDefend = false;

    }

    public void Attack()
    {
        //TODO: Do Attack
        transform.LookAt(player);
        if (!towardsPlayer)
        {
            anim.SetBool("Both Rapid Attack", true);
            player.GetComponent<FPSMovementScript>().GetHitFromBoss();

        }
        else
            anim.SetBool("Spin Attack", true);
    }

    public void EnterDefend()
    {
        inDefendState = true;
        anim.SetBool("Defend", true);
        Invoke("SwitchBackToWalk", 3.0f);
    }

    public void ExitDefend()
    {
        inDefendState = false;
        anim.SetBool("Defend", false);
        firstHit = false;
    }

    public void CheckIfNeedDefense()
    {
        if (!hasUsedDefend && health < 30 && Time.time > lastHit + timeToSwitchToDefendState)
        {
            hasUsedDefend = true;
            SetState(defendState);
        }
    }

    void SwitchBackToWalk()
    {
        SetState(walkState);
    }

    void SwitchToAttack()
    {
        SetState(attackState);
    }

    private void OnDestroy()
    {
        player.GetComponent<FPSMovementScript>().UpdateScore(50);
    }
}
