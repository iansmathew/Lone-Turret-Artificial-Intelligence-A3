using UnityEngine;
using UnityEngine.AI;
using cakeslice;
using System;

public class MinionScript : MonoBehaviour {
    [Header("Minion Properties")]
    [SerializeField] float speed;
    [SerializeField] int scoreValue = 5;
    float health = 100.0f;
    Transform position;
    Transform captain;

    [Header("Attack State Properties")]
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] Transform[] bulletSpawns;
    [SerializeField] GameObject bulletPrefab;
    bool captainAttackState = false;
    static GameObject player;
    float lastFire = 0;

    [Header("On Select Properties")]
    Outline outlineScript;
    bool mouseIsOver = false;

    //Component References
    protected Animator anim;
    protected Rigidbody rb;
    protected NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        outlineScript = GetComponentInChildren<Outline>();
        outlineScript.eraseRenderer = false;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start () {
        agent.speed = speed;
        
	}

    void Update () {
        if (captain && !position)
            return;

        SetOutlineColor();

        if (captain) //if captain is alive
        {
            if (!captain.GetComponent<CaptainScript>().inAttackState) //If in walk mode
            {
                agent.SetDestination(position.position);
                anim.SetFloat("Forward", 1.0f);
                anim.SetBool("Both Rapid Attack", false);
            }
            else
            {
                anim.SetFloat("Forward", 0.0f);
                anim.SetBool("Both Rapid Attack", true);
                FireGun();
            }
        }
        else
        {
            anim.SetFloat("Forward", 0.0f);
            anim.SetBool("Both Rapid Attack", true);
            FireGun();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pBullet")
            TakeHit(5.0f);
        else if (collision.gameObject.tag == "pRocket")
            TakeHit(50);
    }

    private void OnMouseEnter()
    {
        mouseIsOver = true;
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
    }

    private void OnDestroy()
    {
        player.GetComponent<FPSMovementScript>().UpdateScore(scoreValue);
    }

    private void SetOutlineColor()
    {
        if (mouseIsOver)
            outlineScript.color = 1;
        else if (EnemyManagerScript.Instance.selectedEnemy == this.gameObject)
            outlineScript.color = 2;
        else
            outlineScript.color = 0;
    }

    public void SetSquadPosition(Transform _position)
    {
        position = _position;
    }

    public void SetCaptain(Transform _captain)
    {
        captain = _captain;
    }

    public void TakeHit(float _damage)
    {
        anim.SetTrigger("Take Damage");
        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FireGun()
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
