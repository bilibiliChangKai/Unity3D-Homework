// ========================================================
// 描 述：此类保存Disk的各种information，和构建disk的属性
// 作 者：hza 
// 创建时间：2017/03/28 20:33:20
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskInformation
{
    private Color color;
    private float scale;

    public void processDisk(GameObject _disk)
    {
        color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        _disk.GetComponent<Renderer>().material.color = color;
        // 初始化color

        scale = Random.Range(1f, 2f);
        _disk.transform.localScale *= scale;
        // 初始化大小
    }
    public void processDisk(GameObject _disk, Color _color, float _scale)
    {
        _disk.GetComponent<Renderer>().material.color = _color;
        _disk.transform.localScale *= _scale;
        // 自己设置color和大小
    }
}
