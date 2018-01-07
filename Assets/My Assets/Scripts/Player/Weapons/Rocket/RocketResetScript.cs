using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketResetScript : MonoBehaviour {

    [Header("Rocket Properties")]
    [SerializeField] float bulletLife = 15.0f;

    private void Start()
    {
        Invoke("DestroyBullet", bulletLife);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
            return;
        CancelInvoke();
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
