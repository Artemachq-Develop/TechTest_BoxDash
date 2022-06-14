using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;
    private Vector3 offset;
    private bool isEscape = true;
	
    void Start()
    {
        offset = target.transform.position - transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.transform.Rotate(0, horizontal, 0);

        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);
		
        transform.LookAt(target.transform);

        CursorIsLocked();
    }

    private void CursorIsLocked()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscape = !isEscape;
            if (isEscape)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
