using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using My.Position;
using My.Action;

namespace My.Scene
{
    public enum Status { BLEFT, BRIGHT, BLRING, BRLING, LEFTONOFF, RIGHTONOFF, WIN, LOSE }
    // 以下状态的意思
    // BLEFT:船在左侧
    // BRIGHT:船在右侧
    // BLRING:船从左向右移动中
    // BRLING:船从右向左移动中
    // WIN:游戏胜利
    // LOSE:游戏失败

    public interface ISenceController
    {
        void LoadResources();
        void Pause();
        void Resume();
        Status getStatue();
    }

    public class FirstScene : MonoBehaviour, ISenceController
    {
        public SSActionMassager actionMassager;

        public Stack<GameObject> leftPriests = new Stack<GameObject>();
        public Stack<GameObject> rightPriests = new Stack<GameObject>();
        public Stack<GameObject> leftDevils = new Stack<GameObject>();
        public Stack<GameObject> rightDevils = new Stack<GameObject>();

        GameObject priest;
        GameObject devil;

        public GameObject boat;
        // 船和船上的乘客

        public Status nowStatue;

        //--------------------------我是分割线--------------------
        //以下为monobehaviour

        void Start()
        {
            nowStatue = Status.BLEFT;
            Director.Director director = Director.Director.getInstance();
            director.setScene(this);
        }

        //--------------------我是分割线-------------------------
        //以下是场景构造

        public void LoadResources() // 加载初始物体
        {
            for (int i = 0; i < 3; i++)
            {
                priest = Instantiate<GameObject>(Resources.Load<GameObject>("Priest"),
                        Priest.leftFirstPosition + i * Priest.spaceWithTwo, Quaternion.identity);
                priest.GetComponent<Renderer>().material.color = Color.blue;
                leftPriests.Push(priest); // 实例化三个牧师

                devil = Instantiate<GameObject>(Resources.Load<GameObject>("Devil"),
                        Devil.leftFirstPosition + i * Devil.spaceWithTwo, Quaternion.identity);
                devil.GetComponent<Renderer>().material.color = Color.red;
                leftDevils.Push(devil); // 实例化三个恶魔
            }

            GameObject shore = Instantiate(Resources.Load("Shore"), Shore.leftPosition, Quaternion.identity) as GameObject;
            shore.GetComponent<Renderer>().material.color = Color.white;
            shore = Instantiate(Resources.Load("Shore"), Shore.rightPosition, Quaternion.identity) as GameObject;
            shore.GetComponent<Renderer>().material.color = Color.white;
            // 实例化左岸和右岸边

            boat = Instantiate<GameObject>(Resources.Load<GameObject>("Boat"),
                        Boat.leftPosition, Quaternion.identity);
            boat.GetComponent<Renderer>().material.color = Color.black;  // 实例化船

            Instantiate(Resources.Load("Light"), Vector3.up * 100, Quaternion.Euler(90, 0, 0));
            // 实例化灯光
        }

        public void Pause() // 游戏暂停
        {

        }

        public void Resume() // 游戏继续
        {

        }

        public Status getStatue()
        {
            return nowStatue;
        }
        //--------------我是分割线------------------
        //以下是动作设计

    }
}
