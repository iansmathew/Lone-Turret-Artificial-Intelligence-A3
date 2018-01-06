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

    //Component References
    protected Animator anim;
    protected Rigidbody rb;
    protected NavMeshAgent agent;

    //State Variables
    private State currentState;
    public AttackState attackState;
    public WalkState walkState;

    //Unity Functions
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        walkState = new WalkState(this);
        attackState = new AttackState(this);

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

        //if (Input.GetMouseButton(0))
        //{
        //    Debug.Log("Clicked");
        //}
        //else
        //{
        //    outlineScript.color = 0;
        //}

    }

    private void OnMouseEnter()
    {
        outlineScript.color = 1;
    }

    private void OnMouseExit()
    {
        outlineScript.color = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pBullet")
            TakeHit();
    }

    /* --- AI Functions --- */
    public void SetState(State state)
    {
        if (currentState != null)
            currentState.OnStateExit();

        currentState = state;

        if (currentState != null)
            currentState.OnStateEnter();
    }

    //Shared Functions
    public void TakeHit()
    {
        anim.SetTrigger("Take Damage");
        health -= 5.0f;

        if (health <= 0)
            Destroy(gameObject);
    }

    //Walk State Functions
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

    //Attack State Functions

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


}
