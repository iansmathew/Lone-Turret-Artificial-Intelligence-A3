using UnityEngine;

public class BulletResetScript : MonoBehaviour
{
    [SerializeField] float bulletLife = 4.0f;

    private void OnEnable()
    {
        Invoke("DestroyBullet", bulletLife);
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        GetComponent<TrailRenderer>().Clear();
        CancelInvoke();
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        WeaponPoolerScript.Instance.ReturnObjectToPool(this.gameObject);
    }
}
