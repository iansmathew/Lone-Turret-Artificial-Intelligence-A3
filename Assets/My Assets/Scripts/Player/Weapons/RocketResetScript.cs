using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketResetScript : MonoBehaviour {

    [Header("Rocket Properties")]
    [SerializeField] float bulletLife = 4.0f;
    [SerializeField] float movementSpeed = 5.0f;

    Vector3 arcEnd;
    private void OnEnable()
    {
        Invoke("DestroyBullet", bulletLife);
    }

    private void Update()
    {
        if (arcEnd == Vector3.zero)
        {
            return;
        }

        transform.RotateAround(arcEnd / 2.0f, transform.right, movementSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        CancelInvoke();
        DestroyBullet();
    }

    public void SetArcEnd(Vector3 _arcEnd)
    {
        arcEnd = _arcEnd;
    }

    private void DestroyBullet()
    {
        arcEnd = Vector3.zero;
        RocketPoolerScript.Instance.ReturnObjectToPool(this.gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
