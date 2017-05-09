// ========================================================
// 描 述：用于回合间的计数
// 作 者：hza 
// 创建时间：2017/03/27 22:46:23
// 版 本：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour {

    public Text centerText;

    bool active = false;
    float beginTime;

	// Update is called once per frame
	void Update () {
        if (!active) return;
        // 不活跃

        beginTime -= Time.deltaTime;
        if (beginTime > 0)
        {
            centerText.text = "     " + countingNumber();
        }
        else
        {
            centerText.text = "";
            SceneController scene = Singleton<SceneController>.Instance;
            scene.nowState = Status.GAMING;

            RoundController round = Singleton<RoundController>.Instance;
            round.runRound();

            active = false;
            // 设置为不活跃
        }
    }

    public void beginCount()
    {
        beginTime = 3;
        active = true;
    }

    private int countingNumber()
    {
        return (int)beginTime + 1;
    }
}
