using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float xAxis, yAxis;

    [SerializeField] private Transform FollowCam;

    // Direction of the mouse movement
    [SerializeField] private float sense = 0;


    // Update is called once per frame
    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * sense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * sense;
        yAxis = Mathf.Clamp(yAxis, -80, 80); // 

    }

    private void LateUpdate()
    {
        var localEulerAngles = FollowCam.localEulerAngles;
        localEulerAngles =
            new Vector3(yAxis, localEulerAngles.y, localEulerAngles.z);
        FollowCam.localEulerAngles = localEulerAngles;
        var transform1 = transform;
        var eulerAngles = transform1.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, xAxis, eulerAngles.z);
        transform1.eulerAngles = eulerAngles;
    }


}
