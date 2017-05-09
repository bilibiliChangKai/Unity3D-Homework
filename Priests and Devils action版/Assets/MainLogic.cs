using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Director;
using My.Scene;
using My.FirstAction;

public class MainLogic : MonoBehaviour {
    Director direct;
    IUserAction action;

    float width, height;
    // Use this for initialization
    void Start () {
        direct = Director.getInstance();
        direct.setFPS(60);
        action = ((FirstScene)direct.currentSceneController).actionMassager as IUserAction;
        direct.currentSceneController.LoadResources();
    }
	
	// Update is called once per frame
	void OnGUI () {
        width = Screen.width / 12;
        height = Screen.height / 12;

        //Debug.Log(direct.currentSceneController.getStatue());

        if (direct.currentSceneController.getStatue() == Status.WIN)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - width, Screen.height / 2 - height, width * 2, height * 2), "You Win!:)"))
                action.restart();
        }
        else if (direct.currentSceneController.getStatue() == Status.LOSE)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - width, Screen.height / 2 - height, width * 2, height * 2), "You Lose!:("))
                action.restart();
        }
        else
        {
            if (GUI.RepeatButton(new Rect(10, 10, 120, 30), direct.getBaseCode().GameName))
                GUI.TextArea(new Rect(10, 50, Screen.width - 20, Screen.height / 3 * 2)
                    , direct.getBaseCode().GameRule);
            if (GUI.Button(new Rect(Screen.width / 2 - width, castButtonHeight(1.1f), width * 2, height), "Reset Game"))
                action.restart();
            else if (direct.currentSceneController.getStatue() == Status.BLEFT)
            {
                if (GUI.Button(new Rect(castButtonWidth(20f), castButtonHeight(3f), width, height), "Devil On"))
                    action.leftDevilOnBoat();
                if (GUI.Button(new Rect(castButtonWidth(4.5f), castButtonHeight(3f), width, height), "Priest On"))
                    action.leftPriestOnBoat();
                if (GUI.Button(new Rect(castButtonWidth(2.75f), castButtonHeight(3f), width, height), "Off Boat"))
                    action.passOffBoatToLeft();
                if (GUI.Button(new Rect(castButtonWidth(2.75f), castButtonHeight(1.4f), width, height), "Boat Go"))
                    action.boatLeftToRight();
            }
            else if (direct.currentSceneController.getStatue() == Status.BRIGHT)
            {
                if (GUI.Button(new Rect(castButtonWidth(1.305f), castButtonHeight(3f), width, height), "Devil On"))
                    action.rightDevilOnBoat();
                if (GUI.Button(new Rect(castButtonWidth(1.09f), castButtonHeight(3f), width, height), "Priest On"))
                    action.rightPriestOnBoat();
                if (GUI.Button(new Rect(castButtonWidth(1.59f), castButtonHeight(3f), width, height), "Off Boat"))
                    action.passOffBoatToRight();
                if (GUI.Button(new Rect(castButtonWidth(1.59f), castButtonHeight(1.4f), width, height), "Boat Go"))
                    action.boatRightToLeft();
            }
        }
	}

    //辅助函数
    private float castButtonWidth(float scale)
    {
        return (Screen.width - width) / scale;
    }

    private float castButtonHeight(float scale)
    {
        return (Screen.height - height) / scale;
    }
}
