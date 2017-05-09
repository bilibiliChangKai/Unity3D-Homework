/*
 * 描 述：控制摄像机随鼠标移动的脚本，挂载在摄像机上即可
 * 作 者：hza 
 * 创建时间：2017/04/07 17:01:57
 * 版 本：v 1.0
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseRotate : MonoBehaviour {
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    // 鼠标移动方式
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    // 灵敏度

    public float minimumX = -360F;
    public float maximumX = 360F;
    // X最大偏移

    public float minimumY = -60F;
    public float maximumY = 60F;
    // Y最大偏移

    public bool isVisible = true;
    // 鼠标是否可见

    float rotationY = 0F;
    float rotationX = 0F;

    void Start()
    {
        // Make the rigid body not change rotation  
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = isVisible;
        // 鼠标保持在屏幕中间

        if (axes == RotationAxes.MouseXAndY)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            this.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            this.transform.localEulerAngles = new Vector3(0, rotationX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            this.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }
    }
}
