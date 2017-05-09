/*
 * 描 述：用于加载Row的工厂
 * 作 者：hza 
 * 创建时间：2017/04/07 17:31:37
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowFactory : MonoBehaviour
{

    private static List<GameObject> used = new List<GameObject>();
    // 正在使用的对象链表
    private static List<GameObject> free = new List<GameObject>();
    // 正在空闲的对象链表
    // 此函数表示将Target物体放到一个位置
    public GameObject setObjectOnPos(Vector3 targetposition, Quaternion faceposition)
    {
        if (free.Count == 0)
        {
            GameObject aGameObject = Instantiate(Resources.Load("prefabs/Row")
                , targetposition, faceposition) as GameObject;
            // 新建实例，将位置设置成为targetposition，将面向方向设置成faceposition
            used.Add(aGameObject);
        }
        else
        {
            used.Add(free[0]);
            free.RemoveAt(0);
            used[used.Count - 1].SetActive(true);
            used[used.Count - 1].transform.position = targetposition;
            used[used.Count - 1].transform.localRotation = faceposition;
        }
        return used[used.Count - 1];
    }

    public void freeObject(GameObject oj)
    {
        oj.SetActive(false);
        used.Remove(oj);
        free.Add(oj);
    }

    public void freeAllObject()
    {
        if (used.Count == 0) return;
        for (int i = 0; i < used.Count; i++)
        {
            used[i].SetActive(false);
            free.Add(used[i]);
        }
        used.Clear();
        Debug.Log(used.Count);
        Debug.Log(free.Count);
        // 清除used里面所有物体
    }
}
