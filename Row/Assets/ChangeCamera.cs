/*
 * 描 述：射中后，摄像头拉近，持续两秒
 * 作 者：hza 
 * 创建时间：2017/04/07 14:46:35
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject closeCamera;
    // 引用两个摄像机

    bool active;
    float beginTime;

    private void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update () {
        if (!active) return;

        beginTime -= Time.deltaTime;
        if (beginTime < 0)
        {
            resetCount();
        }
	}

    // 显示近身摄像机，持续2秒
    public void ShowCloseCamera()
    {
        mainCamera.SetActive(false);
        closeCamera.SetActive(true);
        // 显示close摄像机

        runCount();
    }

    void runCount()
    {
        active = true;
        beginTime = 2f;
        // 激活计数按钮
    }

    void resetCount()
    {
        active = false;
        mainCamera.SetActive(true);
        closeCamera.SetActive(false);
    }
}
