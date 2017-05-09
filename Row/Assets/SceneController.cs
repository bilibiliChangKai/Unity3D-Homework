/*
 * 描 述：场景和UI控制
 * 作 者：hza 
 * 创建时间：2017/04/07 17:33:39
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    void Start()
    {
        LoadResources();
    }

    public void LoadResources() // 加载初始物体
    {
        GameObject target = Instantiate(Resources.Load("prefabs/Target"), new Vector3(0, 0, -5), Quaternion.identity) as GameObject;
        Instantiate(Resources.Load("prefabs/Wall"), Vector3.zero, Quaternion.Euler(new Vector3(-90, 0, 0)));
        Instantiate(Resources.Load("prefabs/Floor"), new Vector3(0, -2, -12), Quaternion.identity);
        RowActionManager actionmanager = Singleton<RowActionManager>.Instance;
        actionmanager.target = target;
        // 创建实例
    }

    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.F))
        {
            BaseCode bc = Singleton<BaseCode>.Instance;
            GUI.TextArea(new Rect(Screen.width / 4, Screen.height - 50, Screen.width / 2, 40), bc.GameRule);
        }

        // resetgame
        if (Input.GetKeyDown(KeyCode.R))
        {
            RowFactory fac = Singleton<RowFactory>.Instance;
            fac.freeAllObject();
        }
    }
}
