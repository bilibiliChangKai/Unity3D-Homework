/*
 * 描 述：实现DoTweenCore类，用于实现基本动作类
 * 作 者：hza 
 * 创建时间：2017/05/07 12:59:33
 * 版 本：v 1.0
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.LerpFunctionSpace;
using UnityEditor;

namespace My.DoTween.Core
{
    // get模板委托
    public delegate T MyGetter<out T>();

    // set模板委托
    public delegate void MySetter<in T>(T value);

    // 回调委托
    public delegate void TweenCallBack(Tween tween, object data);

    /// <summary>
    /// 一切动作的基类，扩展Tweener和Sequence
    /// </summary>
    public abstract class Tween 
    {
        public object          id;            // Tween的标识，要进行动作的主体
        public object          userData;      // 用户信息，用于回调函数
        public bool            pause;         // 暂停
        public bool            stop;          // 停止
        public bool            isBegin;       //动作是否开始
        public int             loopTime;      // 动作循环次数
        public TweenCallBack   callfore;      // 前置函数
        public TweenCallBack   callback;      // 回调函数

        // 空构造函数
        protected Tween() {}

        public void Clear()
        {
            id = null;
            userData = null;
            callfore = null;
            callback = null;
        }

        // update函数模板
        public abstract void Update(float dt);

    }

    /// <summary>
    /// Tween的向上继承，保存各种函数，真正的动作基类
    /// </summary>
    public abstract class Tweener : Tween
    {
        public float            curTime;       // 当前时间
        public float            duration;      // 动作持续时间
        public LerpFunctionType lerptype;      // 插值种类
        public bool             isInqueue;     // 是否在Sequence里面 

        public abstract Tweener From();
    }

    /// <summary>
    /// 模板类，用于储存各种模板事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tweener<T> : Tweener
    {
        public MyGetter<T>       getter;        // 起始位置的getter
        public MySetter<T>       setter;        // 起始位置的setter
        public T                 begValue;      // 起始位置 
        public T                 endValue;      // 目标位置
        public ChangeValue       change;        // 改变T的值

        // time范围为0-1
        public delegate T ChangeValue(T from, T to, float time);

        // 构造函数
        public static Tweener<T> Create(MyGetter<T> getter, MySetter<T> setter, T endValue, float duration) {
            Tweener<T> tweener = new Tweener<T>();
            // Tween
            tweener.id           = MyDoTween.GetUniqueTweenerID();
            tweener.userData     = getter();
            tweener.pause        = false;
            tweener.stop         = false;
            tweener.isBegin      = false;
            tweener.loopTime     = 1;
            tweener.callfore     = null;
            tweener.callback     = null;

            // Tweener
            tweener.duration     = duration;
            tweener.isInqueue    = false;
            tweener.curTime      = 0;
            tweener.lerptype     = 0;

            // tweener<T>
            tweener.getter       = getter;
            tweener.setter       = setter;
            tweener.begValue     = getter();
            tweener.endValue     = endValue;

            return tweener;
        }

        // 使动作反转
        public override Tweener From()
        {
            // from仅仅建立在动作未开始时
            if (!isBegin)
            {
                // 目标互换
                setter(endValue);
                endValue = begValue;
                begValue = getter();
            }
            return this;
        }

        public override void Update(float dt)
        {
            // 表示动作开始
            if (!isBegin)
            {
                if (callfore != null) callfore(this, userData);
                // 更新物体位置
                begValue = getter();
                isBegin = true;
            }

            if (curTime < duration) {
                if (curTime + dt >= duration)
                {
                    // 如果达到目标值，设置最终地点
                    setter(endValue);
                    if (--loopTime == 0)
                    {
                        if (callback != null) callback(this, userData);
                        stop = true;
                    }
                    else
                    {
                        curTime = 0;
                        setter(begValue);
                    }
                    return;
                }

                // 设置属性值
                setter(change(begValue, endValue, curTime / duration));

                // 增加时间
                curTime += dt;
            }
        }
    }

    /// <summary>
    /// 序列类，用于动作的连续进行
    /// </summary>
    public class Sequence : Tween
    {
        public List<object> tweenActions;  // 动作，函数队列
        public int currentObjectIndex;  // 当前动作位置
        public object currentObject;  // 当前动作，或函数

        // 空构造函数
        public static Sequence create()
        {
            Sequence sequence = new Sequence();

            sequence.id                 = MyDoTween.GetUniqueSequenceID();
            sequence.userData           = null;
            sequence.pause              = false;
            sequence.stop               = false;
            sequence.isBegin            = false;
            sequence.loopTime           = 1;
            sequence.callfore           = null;
            sequence.callback           = null;

            sequence.tweenActions       = new List<object>();
            sequence.currentObjectIndex = 0;
            sequence.currentObject      = null;

            return sequence;
        }

        public override void Update(float dt)
        {
            // 表示动作开始
            if (!isBegin)
            {
                if (callfore != null) callfore(this, userData);
                isBegin = true;
            }

            // 如果暂停，返回
            if (pause) return;

            // 在列表内
            if (currentObjectIndex >= 0 && currentObjectIndex < tweenActions.Count)
            {
                
                // 设置当前Object
                if (currentObject == null) currentObject = tweenActions[currentObjectIndex];
                // 判断Object种类
                if (currentObject is Tween)
                {
                    Tweener currentAction = (Tweener)currentObject;
                    // 动作完成
                    if (currentAction.stop)
                    {
                        currentObject = null;
                        currentObjectIndex++;
                        return;
                    }
                    // 动作暂停
                    if (currentAction.pause) return;
                    // 执行动作
                    currentAction.Update(dt);
                }
                else
                {
                    TweenCallBack callback = (TweenCallBack)currentObject;
                    currentObject = null;
                    currentObjectIndex++;
                    callback(this, this.userData);
                }
            }
            // 动作执行完成
            else if (currentObjectIndex >= tweenActions.Count)
            {
                if (--loopTime == 0)
                {
                    if (callback != null) callback(this, userData);
                    stop = true;
                }
                else
                {
                    foreach (Tweener tweener in tweenActions)
                    {
                        tweener.stop = false;
                        tweener.curTime = 0;
                    }
                    currentObjectIndex = 0;
                }
            }
        }
    }
}


