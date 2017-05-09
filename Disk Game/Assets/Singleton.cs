// ========================================================
// 描 述：这是一个单实例模板，用来引用别的继承monobehaviour的单实例类   
// 作 者：hza 
// 创建时间：2017/03/27 00:02:13
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    // 私有static变量
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 在场景里寻找该类
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                        " is needed in the scene, but it not!");
                }
            }
            return instance;
        }
    }
}