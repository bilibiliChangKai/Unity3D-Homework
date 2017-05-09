/*
 * 描 述：用于控制箭发射的动作类
 * 作 者：hza 
 * 创建时间：2017/04/07 18:23:30
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tem.Action;
using UnityEngine.UI;

public class RowAction : SSAction
{
    private Vector3 beginV;

    public static RowAction GetRowAction(Vector3 beginV)
    {
        RowAction currentAction = ScriptableObject.CreateInstance<RowAction>();
        currentAction.beginV = beginV;
        return currentAction;
    }

    public override void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = beginV;
        // 设置初始速度
    }

    public override void FixedUpdate()
    {
        // 如果掉下去，返回
        if (this.transform.position.y < -2)
        {
            this.destory = true;
            this.callback.SSEventAction(this);
            // 进行回调操作
        }
    }
}


public class RowActionManager : PYActionManager, ISSActionCallback {

    public GameObject cam;
    public GameObject target;
    public Text scoretext;

    private SceneController scene;
    // 控制该动作的场景

    // Use this for initialization
    new void Start () {
        scene = Singleton<SceneController>.Instance;
    }
    
    // Update is called once per frame
    new void FixedUpdate () {
        base.FixedUpdate();
        if (cam.activeSelf && Input.GetMouseButtonDown(0))
            // 左键点击
        {
            RowFactory fac = Singleton<RowFactory>.Instance;
            GameObject row = fac.setObjectOnPos(cam.transform.position, cam.transform.localRotation);
            if (row.GetComponent<Rigidbody>() == null) row.AddComponent<Rigidbody>();
            row.transform.FindChild("RowHead").gameObject.SetActive(true);
            Debug.Log(row.transform.FindChild("RowHead").gameObject.activeSelf);
            Transform head = row.transform.FindChild("RowHead").FindChild("OutCone");
            if (head.gameObject.GetComponent<RowHeadTrigger>() == null)
            {
                head.gameObject.AddComponent<RowHeadTrigger>();
                head.gameObject.GetComponent<RowHeadTrigger>().Target = target;
                head.gameObject.GetComponent<RowHeadTrigger>().scoretext = scoretext;
            }
            // 得到物体，如果未添加脚本就设置脚本，并设置active
            RowAction action = RowAction.GetRowAction(cam.transform.forward * 30);
            // 得到动作
            this.runAction(row, action, this);
        }
    }

    // 回调函数
    public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
        int intParam = 0, string strParam = null, Object objParam = null) 
    {
        RowFactory fac = Singleton<RowFactory>.Instance;
        fac.freeObject(source.gameObject);
    }
}