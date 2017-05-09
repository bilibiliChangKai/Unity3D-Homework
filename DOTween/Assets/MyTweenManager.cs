/*
 * 描 述：实现DoTweenManager类，用于挂在在脚本上并调用协程
 * 作 者：hza 
 * 创建时间：2017/05/07 21:25:03
 * 版 本：v 1.0
 */

using My.DoTween;
using My.DoTween.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pair数据结构
public class Pair<T1, T2>
{
    public T1 first;
    public T2 second;

    public Pair(T1 first, T2 second) {
        this.first = first;
        this.second = second;
    }
} 

public class MyTweenManager : MonoBehaviour {

    private static MyTweenManager _instance;
    public static MyTweenManager GetInstance()
    {
        if (_instance == null)
        {
            // 要使用时，创建新空物体并把脚本挂载在上面
            GameObject gameobject = new GameObject("[MyToTweenManager]");
            gameobject.AddComponent<MyTweenManager>();
            _instance = gameobject.GetComponent<MyTweenManager>();
        }
        return _instance;
    }

    // 储存正在进行的tween和对应的协程的Pair，Key值为Tween的id属性
    private static Dictionary<object, Pair<Tween, Coroutine>> tweenDictionary = new Dictionary<object, Pair<Tween, Coroutine>>();

    // 加入新Tween，并创建线程
    public void AddCoroutineAndTween(Tween tween)
    {
        if (tweenDictionary.ContainsKey(tween.id) || tween.id == null)
            throw new InvalidCastException("该动作已经在进行");
        // 如果设置自动播放，则立刻播放
        tween.pause = !MyDoTween.StartAuto;
        // 加入Tween
        tweenDictionary.Add(tween.id, new Pair<Tween, Coroutine>(tween, StartCoroutine(ExcitingProgramming.Todo(this, tween))));
    }

    //删除Tween，并停止线程
    public void DeleteCoroutineAndTween(Tween tween)
    {
        if (!tweenDictionary.ContainsKey(tween.id))
            throw new KeyNotFoundException("不存在该动作！");
        
        // 停止线程
        StopCoroutine(tweenDictionary[tween.id].second);

        // 删除该tween
        tweenDictionary.Remove(tween.id);
    }

    // 寻找字典中所有的Tween
    public List<Tween> GetAllTween()
    {
        List<Tween> tweenList = new List<Tween>();
        foreach (KeyValuePair<object, Pair<Tween, Coroutine>> pair in tweenDictionary)
            tweenList.Add(pair.Value.first);
        return tweenList;
    }

    // 寻找字典中暂停的Tween
    public List<Tween> GetAllPauseTween()
    {
        List<Tween> tweenList = new List<Tween>();
        foreach (KeyValuePair<object, Pair<Tween, Coroutine>> pair in tweenDictionary)
            if (!pair.Value.first.stop && pair.Value.first.pause) tweenList.Add(pair.Value.first);
        return tweenList;
    }

    // 寻找字典中正在播放的Tween
    public List<Tween> GetAllPlayingTween()
    {
        List<Tween> tweenList = new List<Tween>();
        foreach (KeyValuePair<object, Pair<Tween, Coroutine>> pair in tweenDictionary)
            if (!pair.Value.first.stop && !pair.Value.first.pause) tweenList.Add(pair.Value.first);
        return tweenList;
    }
}

// 协程的参数函数
public static class ExcitingProgramming
{
    public static IEnumerator Todo(MyTweenManager manager, Tween tween)
    {
        // 从下一帧开始
        yield return null; 
        // 判断是否stop
        while (!tween.stop)
        {
            if (tween.pause)
            {
                yield return null;
                continue;
            }
            tween.Update(Time.deltaTime);
            yield return null;
        }

        // 清除tween
        manager.DeleteCoroutineAndTween(tween);
        // 删除tween
        if (tween is Tweener) MyDoTween.FreeTweener(tween);
        else MyDoTween.FreeSequence(tween);
    }
}
