using UnityEngine;

public class FPSMovementScript : MonoBehaviour
{

    [Header("Aim Properties")]
    [SerializeField] Transform pivotBase;
    [SerializeField] Transform turretHead;
    [SerializeField] Transform[] bulletSpawn;
    private Vector2 mouseAxis;


    [Header("Firing Properties")]
    public GameObject bulletPrefab;
    public int ammoSize;
    public float fireForce;
    public float fireRate;

    private float lastFire;

    [Header("Raycast Properties")]
    [SerializeField] LayerMask invertGround;
    private RaycastHit hit;

    //Component References
    private Camera cam;
    private WeaponPoolerScript weaponPoolerScript;


    private void Start()
    {
        cam = Camera.main;
        weaponPoolerScript = new WeaponPoolerScript(bulletPrefab, ammoSize);

        invertGround = ~invertGround;
    }

    private void Update()
    {
        AimTurret();
        FireGun();
    }

    private void AimTurret()
    {

        //Aiming the Turret Head / Pivot Base
        Ray vRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(vRay, out hit, 1000.0f);

        Vector3 pivotLookPos = hit.point;
        pivotLookPos.y = pivotBase.position.y;

        pivotBase.LookAt(pivotLookPos); //move the circle
        turretHead.LookAt(hit.point); //aim the head up and down

        foreach (Transform spawn in bulletSpawn)
        {
            spawn.LookAt(hit.point);
        }
    }
    private void FireGun()
    {
        if (Input.GetMouseButton(0) && Time.time > lastFire + fireRate)
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

    //Debug Commands
    private void OnDrawGizmos()
    {
        //Gizmos.DrawRay(bulletSpawn[0].transform.position, bulletSpawn[0].transform.forward * 100.0f);
        //   Gizmos.DrawSphere(hit.point, 5.0f);
    }

}
