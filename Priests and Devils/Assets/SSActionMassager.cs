using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.Action
{
    public enum SSActionEventType : int { STARTED, COMPLETED }

    public interface ISSActionCallback
    {
        void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
            int intParam = 0, string strParam = null, Object objParam = null);
    }

    public class SSAction : ScriptableObject // 动作的基类
    {
        public bool enable = true;
        public bool destory = false;

        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallback callback { get; set; }

        public virtual void Start()
        {
            throw new System.NotImplementedException("Action Start Error!");
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException("Action Update Error!");
        }
    }

    public class CCMoveToAction : SSAction
    {
        public Vector3 target;
        public float speed;

        public static CCMoveToAction GetSSAction(Vector3 _target, float _speed)
        {
            CCMoveToAction currentAction = ScriptableObject.CreateInstance<CCMoveToAction>();
            currentAction.target = _target;
            currentAction.speed = _speed;
            return currentAction;
        }

        public override void Start()
        {

        }

        public override void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.destory = true;
                this.callback.SSEventAction(this);
            }
        }
    }

    public class CCSequenceAction : SSAction, ISSActionCallback
    {
        public List<SSAction> sequence;
        public int repeat = -1;
        public int start = 0;

        public static CCSequenceAction GetSSAction(List<SSAction> _sequence, int _start, int _repead)
        {
            CCSequenceAction actions = ScriptableObject.CreateInstance<CCSequenceAction>();
            actions.sequence = _sequence;
            actions.start = _start;
            actions.repeat = _repead;
            return actions;
        }

        public override void Start()
        {
            foreach (SSAction ac in sequence)
            {
                ac.gameObject = this.gameObject;
                ac.transform = this.transform;
                ac.callback = this;
                ac.Start();
            }
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if (start < sequence.Count) sequence[start].Update();
        }

        public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED,
            int intParam = 0, string strParam = null, Object objParam = null) //通过对callback函数的调用执行下个动作
        {
            source.destory = false; // 当前动作不能销毁（有可能执行下一次）
            this.start++;
            if (this.start >= this.sequence.Count)
            {
                this.start = 0;
                if (this.repeat > 0) repeat--;
                if (this.repeat == 0)
                {
                    this.destory = true;
                    this.callback.SSEventAction(this);
                }
            }
        }

        private void OnDestroy()
        {
            this.destory = true;
        }
    }

    public class SSActionMassager : MonoBehaviour
    {
        private Dictionary<int, SSAction> dictionary = new Dictionary<int, SSAction>();
        private List<SSAction> watingAddAction = new List<SSAction>();
        private List<int> watingDelete = new List<int>();

        protected void Start()
        {

        }

        protected void Update()
        {
            foreach (SSAction ac in watingAddAction) dictionary[ac.GetInstanceID()] = ac;
            watingAddAction.Clear();
            // 将待加入动作加入dictionary执行

            foreach (KeyValuePair<int, SSAction> dic in dictionary)
            {
                SSAction ac = dic.Value;
                if (ac.destory) watingDelete.Add(ac.GetInstanceID());
                else if (ac.enable) ac.Update();
            }
            // 如果要删除，加入要删除的list，否则更新

            foreach (int id in watingDelete)
            {
                SSAction ac = dictionary[id];
                dictionary.Remove(id);
                DestroyObject(ac);
            }
            watingDelete.Clear();
            // 将deletelist中的动作删除
        }

        public void runAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
        {
            action.gameObject = gameObject;
            action.transform = gameObject.transform;
            action.callback = callback;
            watingAddAction.Add(action);
            action.Start();
        }
    }
}