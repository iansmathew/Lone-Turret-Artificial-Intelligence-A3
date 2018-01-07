using UnityEngine;
using UnityEngine.AI;
using cakeslice;

public class MinionScript : MonoBehaviour
{

    protected static GameObject player;

    [Header("Minion Properties")]
    public float health = 100.0f;

    [Header("Walk State Properties")]
    [SerializeField] float speed = 3.5f;
    private Vector3 targetPos;

    [Header("Attack State Properties")]
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] Transform[] bulletSpawns;
    [SerializeField] GameObject bulletPrefab;
    private float lastFire;

    [Header("On Select Properties")]
    Outline outlineScript;
    bool mouseIsOver = false;

    //Component References
    protected Animator anim;
    protected Rigidbody rb;
    protected NavMeshAgent agent;

    //State Variables
    private State currentState;
    public AttackState attackState;
    public WalkState walkState;
    public DeathState deathState;

    //Unity Functions
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        walkState = new WalkState(this);
        attackState = new AttackState(this);
        deathState = new DeathState(this);

        outlineScript = GetComponentInChildren<Outline>();
    }

    private void Start()
    {
        agent.speed = speed;
        SetState(new WalkState(this));
        outlineScript.eraseRenderer = false;
    }

    private void Update()
    {
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
        if (collision.gameObject.tag == "pBullet")
            TakeHit(5);
        else if (collision.gameObject.tag == "pRocket")
            TakeHit(50);
    }

    /* --- AI Functions --- */
    #region
    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    //Shared Functions
    public void TakeHit(int _damage)
    {
        anim.SetTrigger("Take Damage");
        health -= _damage;
        if (health <= 0)
        {
            SetState(deathState);
        }
    }

    //Walk State Functions
    #region
    public void EnterWalk()
    {
        agent.isStopped = false;
        targetPos = EnemyManagerScript.Instance.RequestCoverPoint();
        anim.SetFloat("Forward", 1.0f);
        agent.SetDestination(targetPos);
    }
    public void ExitWalk()
    {
        anim.SetFloat("Forward", 0.0f);
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
    #endregion

    //Attack State Functions
    #region
    public void EnterAttack()
    {
        anim.SetBool("Both Rapid Attack", true);
        Invoke("SwitchToWalk", 4.0f);
    }

    public void ExitAttack()
    {
        anim.SetBool("Both Rapid Attack", false);
    }

    void SwitchToWalk()
    {
        SetState(walkState);
    }

    public void FireGun()
    {
        Vector3 targetPos = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z); //Create a vector with target position where y = 0;
        transform.LookAt(targetPos); //look at target

        if (Time.time > lastFire + fireRate) //Fire at intervals
        {
            foreach (Transform spawn in bulletSpawns)
            {
                Vector3 pos = spawn.position; //If you need to modify spawn position change here
                GameObject bullet = Instantiate(bulletPrefab, pos, spawn.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000.0f);
            }
            lastFire = Time.time;
        }
    }
    #endregion

    //Death State Functions
    #region
    public void EnterDeath()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject);
    }
    #endregion

    #endregion
}
