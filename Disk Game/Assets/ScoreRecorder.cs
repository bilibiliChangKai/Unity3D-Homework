// ========================================================
// 描 述：用来计算得分，加减分数后将得分面板刷新
// 作 者：hza 
// 创建时间：2017/03/29 21:53:48
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorder {

    public Text scoreText;
    // 计分板
  
    private int score;
    // 纪录分数

    public ScoreRecorder(Text _scoreText)
    {
        scoreText = _scoreText;
    }

    public void resetScore()
    {
        score = 0;
    }

    // 飞碟点击中加分
    public void addScore(int addscore)
    {
        score += addscore;
        scoreText.text = "Score:" + score;
    }

    public void setDisActive()
    {
        scoreText.text = "";
    }

    public void setActive()
    {
        scoreText.text = "Score:" + score;
    }
}
