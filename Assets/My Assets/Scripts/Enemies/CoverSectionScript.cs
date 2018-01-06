using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSectionScript : MonoBehaviour
{

    public float minSizeX
    {
        get { return transform.position.x - (x / 2.0f); } //get left most point
    }

    public float maxSizeX
    {
        get { return transform.position.x + (x / 2.0f); } //get right most point
    }

    public float minSizeZ
    {
        get { return transform.position.z - (z / 2.0f); } //get backward point
    }

    public float maxSizeZ
    {
        get { return transform.position.z + (z / 2.0f); } //get front most point
    }


    [SerializeField] float x = 10.0f; //sets width
    [SerializeField] float z = 10.0f; //sets breadth

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(x, 0.1f, z));
    }
}
