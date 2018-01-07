using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSMovementScript : MonoBehaviour
{
    //TODO: Add obstacles to game
    [Header("General Properties")]
    [SerializeField] Image healthBar;
    [SerializeField] float health = 100.0f;


    [Header("Aim Properties")]
    [SerializeField] Transform pivotBase;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform[] bulletSpawn;
    private Transform lookAtObj = null;
    private Vector3 hitPoint;

    [Header("Raycast Properties")]
    [SerializeField] LayerMask invertCharacterMask;

    [Header("Normal Bullet Properties")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletAmmoSize;
    [SerializeField] float bulletFireForce;
    [SerializeField] float bulletFireRate;
    
    private WeaponPoolerScript bulletWeaponPool;

    [Header("Rocket Properties")]
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] int rocketAmmoSize;
    [SerializeField] float rocketFireForce;
    [SerializeField] float rocketFireRate;
    [HideInInspector] public bool canFire = false;

    private RocketPoolerScript rocketWeaponPool;
    private float lastFire;
    private int currentWeapon = 0;

    [Header("Score Properties")]
    [SerializeField] Text scoreText;
    float score = 0;

    [Header("Game Over Properties")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text gameOverText;
    
    //Component References
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
        bulletWeaponPool = new WeaponPoolerScript(bulletPrefab, bulletAmmoSize);
        //rocketWeaponPool = new RocketPoolerScript(rocketPrefab, rocketAmmoSize);
        invertCharacterMask = ~invertCharacterMask;

    }

    private void Update()
    {
        OnClick();
        AimTowardsPos();
        FireGun();
        CheckIfSwitchWeapons();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(0.1f);
        }
    }

    /// <summary>
    /// Check if the player switches weapons
    /// </summary>
    private void CheckIfSwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = 1;
    }

    /// <summary>
    /// Locks on to enemy and starts firing
    /// </summary>
    private void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetLookAtPos();
            StartFiring();
        }
    }

    /// <summary>
    /// Toggles firing bool
    /// </summary>
    private void StartFiring()
    {
        canFire = true;
    }

    /// <summary>
    /// Gets the enemy that player clicked on and sets it to lookAtObj
    /// </summary>
    private void GetLookAtPos()
    {
        Ray camScreenRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(camScreenRay, out hit, 500, invertCharacterMask))
        {
            lookAtObj = hit.transform.root.transform;
            hitPoint = hit.point;
            EnemyManagerScript.Instance.selectedEnemy = hit.transform.gameObject;
        }
    }

    /// <summary>
    /// Aims the turret towatds lookAtObj
    /// </summary>
    private void AimTowardsPos()
    {
        //Do not run if you haven't clicked anything
        if (!lookAtObj)
            return;

        Vector3 pivotBaseLookAt = Vector3.zero;
        Vector3 turretHeadLookAt = Vector3.zero;

        if (lookAtObj.tag == "Ground")
        {
            pivotBaseLookAt = hitPoint;
            turretHeadLookAt = hitPoint;
        }
        else
        {
            pivotBaseLookAt = lookAtObj.position + lookAtObj.forward * 2.5f;
            turretHeadLookAt = lookAtObj.position;
            //turretHeadLookAt.y = 1.5f; //TODO: Look at actual mid point of enemy
        }

        pivotBaseLookAt.y = pivotBase.position.y; //Corrects pivot base so it doesn't tilt
        pivotBase.LookAt(pivotBaseLookAt);

        turretHead.LookAt(turretHeadLookAt);



    }

    /// <summary>
    /// Fires the current gun
    /// </summary>
    private void FireGun()
    {
        if (currentWeapon == 0)
            FireBullet();
        else if (currentWeapon == 1)
            FireRocket();
    }

    private void FireBullet()
    {
        if (canFire && Time.time > lastFire)
        {
            lastFire = Time.time + bulletFireRate;

            for (int i = 0; i < bulletSpawn.Length; i++)
            {
                GameObject bullet = WeaponPoolerScript.Instance.GetObjectFromPool();
                if (!bullet)
                    return;
                bullet.GetComponent<TrailRenderer>().Clear();
                bullet.transform.position = bulletSpawn[i].position;
                bullet.transform.rotation = bulletSpawn[i].rotation;
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), transform.GetComponent<Collider>());

                bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn[i].forward * bulletFireForce);
            }


        }
    }

    //TODO: Implement second weapon
    private void FireRocket()
    {
        if (canFire && Time.time > lastFire)
        {
            lastFire = Time.time + rocketFireRate;
            GameObject rocket = Instantiate(rocketPrefab, bulletSpawn[1].position, bulletSpawn[1].rotation) as GameObject;
            Physics.IgnoreCollision(rocket.GetComponent<Collider>(), transform.GetComponent<Collider>());
            rocket.GetComponent<RocketMovementScript>().SetRocketTarget(lookAtObj.transform);
        }
    }

    /// <summary>
    /// Decreased health by damage and scales healthbar
    /// </summary>
    public void TakeDamage(float damage)
    {
        health -= damage;
        float mappedHealth = CustomFunc.Remap(health, 0, 100, 0, 1);
        healthBar.transform.localScale = new Vector3(mappedHealth, healthBar.transform.localScale.y, 0);

        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "Defeat..";
            Destroy(gameObject);
        }
    }

    public void UpdateScore(int _score)
    {
        score += _score;
        scoreText.text = "Score: " + score.ToString();
    }

}