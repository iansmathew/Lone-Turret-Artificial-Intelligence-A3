using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour {
    [SerializeField] Transform cameraHolder;
    [SerializeField] float horizontalSpeed = 2.0f;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            cameraHolder.Rotate(cameraHolder.up, h);
        }
    }

}
