/*
 * 描 述：放在rowhead物体上，写触发函数
 * 作 者：hza 
 * 创建时间：2017/04/07 13:57:17
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RowHeadTrigger : MonoBehaviour {

    public GameObject Target;
    public Text scoretext;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void OnTriggerEnter(Collider e)
    {
        if (e.gameObject.tag == "Round")
        {
            float realDistance = Vector2.Distance(this.transform.position, Target.transform.position);
            float imagDistance = (1 + (e.gameObject.name[5] - '1') * 2) * 0.5f;
            int score;
            if (realDistance + 0.1 < imagDistance) score = (6 - (int)(realDistance * 10)) * 10;
            else score = (e.gameObject.name[5] - '0') * 10;

            ScoreRecorder scorerecorder = ScoreRecorder.getInstance(scoretext);
            scorerecorder.addScore(score);
            // 添加分数

            Destroy(this.transform.parent.parent.GetComponent<Rigidbody>());

            ChangeCamera ch = Singleton<ChangeCamera>.Instance;
            ch.ShowCloseCamera();
            // 切换相机
        }
    }
}
