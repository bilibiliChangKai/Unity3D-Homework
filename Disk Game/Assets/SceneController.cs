// ========================================================
// 描 述：场景控制，主要控制开始结束按钮和场景启动
// 作 者：hza 
// 创建时间：2017/03/27 00:21:36
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Disk;
using UnityEngine.UI;

public enum Status { BEGIN, COUNTING, WATING, GAMING, OVER }
public class SceneController : MonoBehaviour
{
    public ParticleSystem boom;
    public Text centerText;
    public Text scoreText;
    public Text roundText;

    public ScoreRecorder recorder;
    RoundController round;

    public Status nowState;

    void Start()
    {
        nowState = Status.BEGIN;
        centerText.text = "";
        roundText.text = "";
        recorder = new ScoreRecorder(scoreText);
        round = Singleton<RoundController>.Instance;

        round.resetRound();
        recorder.setDisActive();
        // 初始设置
    }

    private void OnGUI()
    {
        if (nowState == Status.BEGIN)
        {
            if (GUI.RepeatButton(new Rect(10, 10, 100, 30), "Game Rule"))
                GUI.TextArea(new Rect(120, 10, Screen.width / 2, 50), "点击空格开始关卡，鼠标点击射击，击中所有目标即可进行下一关，右上角可以调节运动学和物理学模式，物理学模式会造成碰撞。");
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Play Game"))
            {
                nowState = Status.WATING;
                recorder.resetScore();
            }
        }
        else if (nowState == Status.OVER)
        {
            if (GUI.RepeatButton(new Rect(Screen.width / 2 - 50, Screen.height * 3 / 4 - 25, 100, 50), "Reset Game"))
                restart();
        }
    }

    public void restart()
    {
        round.resetRound();
        centerText.text = "";
        recorder.setDisActive();
        roundText.text = "";
        nowState = Status.BEGIN;
    }
}
