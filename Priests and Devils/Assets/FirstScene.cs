using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using My.Position;


namespace My.Scene
{
    public enum Status { BLEFT, BRIGHT, BLRING, BRLING, WIN, LOSE}
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

    public interface IUserAction
    {
        void boatLeftToRight();
        void boatRightToLeft();
        void devilOnBoat();
        void priestOnBoat();
        void passOffBoat();
        void restart();
        void checkEnding();
    }

    public class FirstScene : MonoBehaviour, ISenceController, IUserAction
    {
        public Stack<GameObject> leftPriests = new Stack<GameObject>();
        public Stack<GameObject> rightPriests = new Stack<GameObject>();
        public Stack<GameObject> leftDevils = new Stack<GameObject>();
        public Stack<GameObject> rightDevils = new Stack<GameObject>();

        GameObject priest;
        GameObject devil;

        GameObject boat;
        GameObject[] boatPass = new GameObject[2];
        string[] boatPassName = new string[2];
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

        void Update() // 进行动作操作
        {
            //Debug.Log("enter?");
            if (nowStatue == Status.BLRING) // 进行船从左到右的动作
            {
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, Boat.rightPosition, Boat.Speed * Time.deltaTime);
                if (boat.transform.position == Boat.rightPosition) nowStatue = Status.BRIGHT;
            }
            else if (nowStatue == Status.BRLING) // 进行船从右到左的动作
            {
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, Boat.leftPosition, Boat.Speed * Time.deltaTime);
                if (boat.transform.position == Boat.leftPosition) nowStatue = Status.BLEFT;
            }
            else checkEnding();
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

            GameObject  shore = Instantiate(Resources.Load("Shore"), Shore.leftPosition, Quaternion.identity) as GameObject;
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

        public void boatLeftToRight() // 如果船在左边并且船上有人，船开向右边
        {
            if (nowStatue == Status.BLEFT && boatSize() != 0)
                nowStatue = Status.BLRING; // 具体动作在update执行
        }

        public void boatRightToLeft() // 同上
        {
            if (nowStatue == Status.BRIGHT && boatSize() != 0)
                nowStatue = Status.BRLING;
        }

        public void devilOnBoat() // 如果船在岸边并且人没满，恶魔上船
        {
            if (boatSize() != 2)
            {
                int pos = (boatPass[0] == null ? 0 : 1); // 看要插入的是左还是右
                if (nowStatue == Status.BLEFT)
                {
                    if (leftDevils.Count == 0) return;
                    boatPass[pos] = leftDevils.Pop(); // 左岸恶魔移动到船上
                }
                else
                {
                    if (rightDevils.Count == 0) return;
                    boatPass[pos] = rightDevils.Pop(); // 右岸恶魔移动到船上
                }

                boatPassName[pos] = "devil"; // 设置姓名
                boatPass[pos].transform.position = boat.transform.position + (pos == 0 ? Boat.leftSeat : Boat.rightSeat); // 设置位置 
                boatPass[pos].transform.parent = boat.transform; // 设置关系
            }
           
        }

        public void priestOnBoat() // 同上
        {
            if (boatSize() != 2)
            {
                int pos = (boatPass[0] == null ? 0 : 1); // 看要插入的是左还是右

                if (nowStatue == Status.BLEFT)
                {
                    if (leftPriests.Count == 0) return;
                    boatPass[pos] = leftPriests.Pop(); // 左岸牧师移动到船上
                }
                else
                {
                    if (rightPriests.Count == 0) return;
                    boatPass[pos] = rightPriests.Pop(); // 右岸牧师移动到船上
                }

                boatPassName[pos] = "priest"; // 设置姓名
                boatPass[pos].transform.position = boat.transform.position + (pos == 0 ? Boat.leftSeat : Boat.rightSeat); // 设置位置 
                boatPass[pos].transform.parent = boat.transform; // 设置关系
            }
        }

        public void passOffBoat() // 如果船在岸边并且船不空
        {
            if (boatSize() != 0)
            {
                int pos = boatSize() - 1;
                if (boatPassName[pos] == "devil") devilOffBoat(pos);
                else priestOffBoat(pos);
            }
        }

        public void restart()
        {
            SceneManager.LoadScene("PriestsAndDevils");
            nowStatue = Status.BLEFT;
        }

        public void checkEnding()
        {
            if (rightDevils.Count == 3 && rightPriests.Count == 3) // 如果牧师和魔鬼都到右边
            {
                nowStatue = Status.WIN;
                return;
            }

            int bpri = 0, bdev = 0;
            int ld = 0, rd = 0, lp = 0, rp = 0;

            for (int i = 0; i < 2; i++) // 记录船上牧师与魔鬼的数量
            {
                if (boatPass[i] != null)
                    if (boatPassName[i] == "devil") bdev++;
                    else bpri++;
            }

            if (nowStatue == Status.BLEFT) // 船在左边的胜利条件
            {
                ld = leftDevils.Count + bdev;
                lp = leftPriests.Count + bpri;
                rd = rightDevils.Count;
                rp = rightPriests.Count;
            }
            else if (nowStatue == Status.BRIGHT) // 船在右边的胜利条件
            {
                ld = leftDevils.Count;
                lp = leftPriests.Count;
                rd = rightDevils.Count + bdev;
                rp = rightPriests.Count + bpri;
            }
            if (lp != 0 && ld > lp) nowStatue = Status.LOSE;
            if (rp != 0 && rd > rp) nowStatue = Status.LOSE;
        }

        // 辅助函数
        private int boatSize()
        {
            int sum = 0;
            if (boatPass[0] != null) sum++;
            if (boatPass[1] != null) sum++;
            return sum;
        }

        private void devilOffBoat(int pos)
        {
            boatPass[pos].transform.parent = null;
            if (nowStatue == Status.BLEFT)
            {
                // action
                boatPass[pos].transform.position = Devil.leftFirstPosition + Devil.spaceWithTwo * leftDevils.Count;
                // 设置位置
                leftDevils.Push(boatPass[pos]); // 船上恶魔移动到左岸上
            }
            else
            {
                // action
                boatPass[pos].transform.position = Devil.rightFirstPosition + Devil.spaceWithTwo * rightDevils.Count;
                // 设置位置
                rightDevils.Push(boatPass[pos]); // 船上恶魔移动到右岸上
            }
            boatPass[pos] = null; // 清除船上物体
        }

        private void priestOffBoat(int pos)
        {
            boatPass[pos].transform.parent = null;
            if (nowStatue == Status.BLEFT)
            {
                // action
                boatPass[pos].transform.position = Priest.leftFirstPosition + Priest.spaceWithTwo * leftPriests.Count;
                // 设置位置
                leftPriests.Push(boatPass[pos]); // 船上天使移动到左岸上
            }
            else
            {
                // action
                boatPass[pos].transform.position = Priest.rightFirstPosition + Priest.spaceWithTwo * rightPriests.Count;
                // 设置位置
                rightPriests.Push(boatPass[pos]); // 船上天使移动到右岸上
            }
            boatPass[pos] = null; // 清除船上物体
        }
    }
}

