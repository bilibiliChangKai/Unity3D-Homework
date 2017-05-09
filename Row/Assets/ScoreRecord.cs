/*
 * 描 述：用于计算分数
 * 作 者：hza 
 * 创建时间：2017/04/07 20:03:57
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorder
{
    public Text scoreText;
    // 计分板

    private int score;
    // 纪录分数

    private static ScoreRecorder _instance;
    public static ScoreRecorder getInstance(Text _scoreText)
    {
        if (_instance == null)
        {
            _instance = new ScoreRecorder();
            _instance.scoreText = _scoreText;
            _instance.resetScore();
        }
        return _instance;
    }
    // 单实例

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

