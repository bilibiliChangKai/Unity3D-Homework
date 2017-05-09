/*
 * 描 述：设置游戏基本信息
 * 作 者：hza 
 * 创建时间：2017/04/07 20:29:47
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCode : MonoBehaviour
{
    public string GameName;
    public string GameRule;

    void Start()
    {
        GameName = "Shoot";
        GameRule = "左键点击射箭，射中靶子加分，靶心从内到外分别为60-10分，点击clear清除所有的箭";
    }
}

