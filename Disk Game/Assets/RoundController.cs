// ========================================================
// 描 述：用于回合数控制，回合循环
// 作 者：hza 
// 创建时间：2017/04/02 20:22:30
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundController : MonoBehaviour {

    public bool isPhysics;

    public GameObject cam;
    public Text roundText;
    public Text centerText;
    public Text modelText;
    // 外部引用

    SceneController scene;
    Count count;
    ScoreRecorder recorder;
    DiskActionManager actionManager;
    PhysicsDiskActionManager physicsActionManager;
    // 类间引用

    int round;

    // Use this for initialization
    void Start () {
        isPhysics = false;
        scene = Singleton<SceneController>.Instance;
        count = Singleton<Count>.Instance;
        actionManager = Singleton<DiskActionManager>.Instance;
        physicsActionManager = Singleton<PhysicsDiskActionManager>.Instance;
        recorder = scene.recorder;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            // 获取射线
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Text")
            // 如果点击的物体是ModelText
            {
                ChangeActionModel();
            }
        }
        if (scene.nowState == Status.WATING)
            // 等待阶段
        {
            centerText.text = "Round:" + round;
            if (Input.GetKeyDown("space"))
            {
                count.beginCount();
                ResumeRecord();
                scene.nowState = Status.COUNTING;
                // 开始计数
            }
        }
    }

    public void runRound()
    {
        // 判断条件并且进行动作
        if (isPhysics) physicsActionManager.runActionByRound(round);
        else actionManager.runActionByRound(round);
    }

    public void NextRound()
    {
        round++;
    }

    public void resetRound()
    {
        round = 1;
    }

    // 用于暂停显示回合数和当前分数
    public void PauseRecord()
    {
        roundText.text = "";
        recorder.setDisActive();
    }

    // 用于恢复显示回合数和当前分数
    public void ResumeRecord()
    {
        roundText.text = "Round:" + round;
        recorder.setActive();
    }

    public void ChangeActionModel()
    {
        isPhysics = !isPhysics;
        modelText.text = "Model:" + (isPhysics ? "Phys" : "Kine");
    } 
}
