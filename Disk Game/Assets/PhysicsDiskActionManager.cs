// ========================================================
// 描 述：运用物理学的DiskActionManager
// 作 者：hza 
// 创建时间：2017/04/06 22:35:00
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tem.Action;
using My.Disk;
using UnityEngine.UI;

public class PhysicsFlyAction : SSAction
{
    public Vector3 firstPower;
    // 初始飞行力

    public static PhysicsFlyAction GetSSAction(Vector3 _firstPower)
    {
        PhysicsFlyAction currentAction = ScriptableObject.CreateInstance<PhysicsFlyAction>();
        currentAction.firstPower = _firstPower;
        return currentAction;
    }

    public override void Start()
    {
        Rigidbody rigid = this.gameObject.AddComponent<Rigidbody>();
        rigid.velocity = firstPower;
    }

    public override void FixedUpdate()
    {
        if (!this.gameObject.activeSelf) // 如果飞碟已经回收
        {
            this.destory = true;
            this.callback.SSEventAction(this, SSActionEventType.STARTED);
            return;
        }
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

public class PhysicsDiskActionManager : SSPhysicsActionManager, ISSActionCallback {

    public GameObject cam;
    public Text centerText;

    DiskFactory dic;
    Vector3 leftEmitPos = new Vector3(-15, 10, -5);
    Vector3 rightRmitPos = new Vector3(15, 10, -5);

    int sumNum;
    bool isLose;

    new void Start()
    {
        dic = Singleton<DiskFactory>.Instance;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // update一样的进行
    private void Update()
    {
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

            PhysicsFlyAction fly = PhysicsFlyAction.GetSSAction(new Vector3(Random.Range(5f, 20), Random.Range(2.5f, 5), Random.Range(0, 0.75f)));
            this.runAction(disk, fly, this);
        }

        for (int i = 1; i < round; i += 2)
        {
            disk = dic.setDiskOnPos(rightRmitPos);

            PhysicsFlyAction fly = PhysicsFlyAction.GetSSAction(new Vector3(Random.Range(-20f, -5f), Random.Range(2.5f, 5), Random.Range(0, 0.75f)));
            this.runAction(disk, fly, this);
        }
        // 设置飞碟发射，发射round个飞碟
    }

    // 回调函数
    public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
        int intParam = 0, string strParam = null, Object objParam = null)
    {
        // 销毁刚体脚本
        Destroy(source.gameObject.GetComponent<Rigidbody>());
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