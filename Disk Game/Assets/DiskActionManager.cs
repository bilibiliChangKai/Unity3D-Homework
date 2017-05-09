// ========================================================
// 描 述：用于管理飞碟飞行，其中包含FlyAction类
// 作 者：hza 
// 创建时间：2017/04/01 14:25:14
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tem.Action;
using My.Disk;
using UnityEngine.UI;

public class FlyAction : SSAction
{
    public Vector3 firstDirect;
    // 飞行方向
    private float time;
    // 已经飞行时间

    private static float g = 5f;
    public static FlyAction GetSSAction(Vector3 _firstDirect)
    {
        FlyAction currentAction = ScriptableObject.CreateInstance<FlyAction>();
        currentAction.firstDirect = _firstDirect;
        currentAction.time = 0;
        return currentAction;
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        if (!this.gameObject.activeSelf) // 如果飞碟已经回收
        {
            this.destory = true;
            this.callback.SSEventAction(this, SSActionEventType.STARTED);
            return;
        }
        time += Time.deltaTime;
        transform.position += Time.deltaTime * firstDirect;
        // 各个方向的匀速运动
        transform.position += Vector3.down * g * time * Time.deltaTime;
        // 竖直方向的匀加速运动
        if (this.transform.position.y < 0)
        {
            this.destory = true;
            DiskFactory fac = Singleton<DiskFactory>.Instance;
            fac.freeDisk(gameObject);
            // 回收飞碟
            this.callback.SSEventAction(this);
        }
    }
}

public class DiskActionManager : SSActionManager, ISSActionCallback {

    public GameObject cam;
    public Text centerText;

    DiskFactory dic;
    Vector3 leftEmitPos = new Vector3(-15, 10, -5);
    Vector3 rightRmitPos = new Vector3(15, 10, -5);

    int sumNum;
    bool isLose;

    new void Start () {
        dic = Singleton<DiskFactory>.Instance;
    }
    
    // Update is called once per frame
    new void Update () {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            // 获取射线
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Disk")
            // 如果点击的物体是Disk
            {
                SceneController scene = Singleton<SceneController>.Instance;
                scene.recorder.addScore(10); // 射中+10分

                /*boom.transform.position = hit.collider.gameObject.transform.position;
                boom.GetComponent<Renderer>().material.color = hit.collider.GetComponent<Renderer>().material.color;
                boom.Play();
                // 粒子爆炸效果*/

                dic.freeDisk(hit.collider.gameObject);
                // 点中，毁掉自身脚本，返回工厂
            }
        }
    }

    public void runActionByRound(int round)
    {
        sumNum = round;
        isLose = false;
        GameObject disk;
        for (int i = 0; i < round; i += 2)
        {
            disk = dic.setDiskOnPos(leftEmitPos);
            
            FlyAction fly = FlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 5), Random.Range(0, 0.75f)));
            this.runAction(disk, fly, this);
        } 

        for (int i = 1; i < round; i += 2)
        {
            disk = dic.setDiskOnPos(rightRmitPos);

            FlyAction fly = FlyAction.GetSSAction(new Vector3(Random.Range(-20f, -5f), Random.Range(2.5f, 5), Random.Range(0, 0.75f)));
            this.runAction(disk, fly, this);
        }
        // 设置飞碟发射，发射round个飞碟
    }

    // 回调函数
    public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
        int intParam = 0, string strParam = null, Object objParam = null) 
    {
        if (events == SSActionEventType.COMPLETED)
            // 落到y轴以下
        {
            isLose = true;
            sumNum--;
        }
        else
        {
            sumNum--;
        }

        if (sumNum == 0)
            // 如果本回合结束
        {
            SceneController scene = Singleton<SceneController>.Instance;
            if (isLose)
            {
                centerText.text = "  LOSE";
                scene.nowState = Status.OVER;
            }
            else
            {
                RoundController round = Singleton<RoundController>.Instance;
                round.NextRound();
                round.PauseRecord();
                scene.nowState = Status.WATING;
            }
        }
    }
}