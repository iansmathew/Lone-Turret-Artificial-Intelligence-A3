using System;
using UnityEngine;

public class FPSMovementScript : MonoBehaviour
{

    [Header("Aim Properties")]
    [SerializeField] Transform pivotBase;
    [SerializeField] Transform turretHead;
    private Transform lookAtObj = null;
    private Vector3 hitPoint;

    [Header("Firing Properties")]
    [HideInInspector] public bool canFire = false;
    [SerializeField] Transform[] bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int ammoSize;
    [SerializeField] float fireForce;
    [SerializeField] float fireRate;

    private WeaponPoolerScript weaponPoolerScript;
    private float lastFire;

    [Header("Raycast Properties")]
    [SerializeField] LayerMask invertCharacterMask;

    //Component References
    private Camera cam;


    private void Start()
    {
        cam = Camera.main;
        weaponPoolerScript = new WeaponPoolerScript(bulletPrefab, ammoSize);
        invertCharacterMask = ~invertCharacterMask;
    }

    private void Update()
    {
        OnClick();
        AimTowardsPos();
        FireGun();
    }

    private void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetLookAtPos();
            StartFiring();
        }
    }

    private void StartFiring()
    {
        canFire = true;
    }

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
            turretHeadLookAt.y = 1.5f; //TODO: Look at actual mid point of enemy
        }

        pivotBaseLookAt.y = pivotBase.position.y; //Corrects pivot base so it doesn't tilt
        pivotBase.LookAt(pivotBaseLookAt);

        turretHead.LookAt(turretHeadLookAt);



    }

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

    private void FireGun()
    {
        if (canFire && Time.time > lastFire + fireRate)
        {
            lastFire = Time.time;

            for (int i = 0; i < bulletSpawn.Length; i++)
            {
                GameObject bullet = WeaponPoolerScript.Instance.GetObjectFromPool();
                bullet.GetComponent<TrailRenderer>().Clear();
                bullet.transform.position = bulletSpawn[i].position;
                bullet.transform.rotation = bulletSpawn[i].rotation;
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), transform.GetComponent<Collider>());

                bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn[i].forward * fireForce);
            }


        }
    }
}
