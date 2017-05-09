/*
 * 描 述：用于各种控件，展示扩展函数，进行链式调用
 * 作 者：hza 
 * 创建时间：2017/05/08 23:09:03
 * 版 本：v 1.0
 */

using My.LerpFunctionSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.DoTween.Core
{
    public static class TweenClassFunction
    {
        // 设置前置函数
        public static T OnStart<T>(this T tween, TweenCallBack _callfore) where T : Tween
        {
            tween.callfore = _callfore;
            return tween;
        }

        // 设置回调函数
        public static T OnComplete<T>(this T tween, TweenCallBack _callback) where T : Tween
        {
            tween.callback = _callback;
            return tween;
        }


        // 设置循环次数，如果循环次数<=0，无限循环
        public static T SetLoopTime<T>(this T tween, int looptime) where T : Tween
        {
            if (looptime < 0) looptime = 0;
            tween.loopTime = looptime;
            return tween;
        }

        // 暂停动作
        public static T Pause<T>(this T tween) where T : Tween
        {
            tween.pause = true;
            return tween;
        }

        // 停止动作
        public static T Stop<T>(this T tween) where T : Tween
        {
            tween.stop = true;
            return tween;
        }
    }
    public static class TweenerClassFunction
    {
        // tweener
        // 是否在队列内
        public static bool isInQueue<T>(this T tweener) where T : Tweener
        {
            return tweener.isInqueue;
        }

        // 设置差值种类
        public static T SetEaseType<T>(this T tweener, LerpFunctionType type) where T : Tweener
        {
            tweener.lerptype = type;
            return tweener;
        }
    }
    public static class SequenceClassFunction
    {
        // sequence
        // 后面添加新节点
        public static T Append<T>(this T sequence, Tweener tweener) where T : Sequence
        {
            MyDoTween.DeleteCoroutineAndTween(tweener);
            tweener.isInqueue = true;
            sequence.tweenActions.Add(tweener);
            return sequence;
        }

        // 在后面添加新的回调函数
        public static T AppendCallback<T>(this T sequence, TweenCallBack callback) where T : Sequence
        {
            sequence.tweenActions.Add(callback);
            return sequence;
        }

        // 在后面添加新的等待时间
        public static T AppendInterval<T>(this T sequence, float time) where T : Sequence
        {
            return sequence.Append(MyDoTween.To(() => time, x => time = x, 0, time));
        }

        // 前面添加新节点
        public static T Prepend<T>(this T sequence, Tweener tweener) where T : Sequence
        {
            MyDoTween.DeleteCoroutineAndTween(tweener);
            tweener.isInqueue = true;
            sequence.tweenActions.Insert(0, tweener);
            return sequence;
        }

        // 在前面添加新的回调函数
        public static T PrependCallback<T>(this T sequence, TweenCallBack callback) where T : Sequence
        {
            sequence.tweenActions.Insert(0, callback);
            return sequence;
        }

        // 在前面添加新的等待时间
        public static T PrependInterval<T>(this T sequence, float time) where T : Sequence
        {
            return sequence.Prepend(MyDoTween.To(() => time, x => time = x, 0, time));
        }

        // 任意位置添加新节点
        public static T Insert<T>(this T sequence, int atPos, Tweener tweener) where T : Sequence
        {

            if (atPos > 0 && atPos < sequence.tweenActions.Count)
            {
                MyDoTween.DeleteCoroutineAndTween(tweener);
                tweener.isInqueue = true;
                sequence.tweenActions.Insert(atPos, tweener);
            }
            return sequence;
        }

        // 任意位置添加新的回调函数
        public static T InsertCallback<T>(this T sequence, int atPos, TweenCallBack callback) where T : Sequence
        {
            if (atPos > 0 && atPos < sequence.tweenActions.Count)
                sequence.tweenActions.Insert(atPos, callback);
            return sequence;
        }
    }
    public static class TransformClassFunction
    {
        // transform
        public static Tweener DOMove(this Transform transform, Vector3 to, float duration)
        {
            return MyDoTween.To(() => transform.position, x => transform.position = x, to, duration);
        }

        public static Tweener DOMoveX(this Transform transform, float to, float duration)
        {
            MyGetter<float> getter = () =>
            {
                return transform.position.x;
            };
            MySetter<float> setter = x =>
            {
                transform.position = new Vector3(x, transform.position.y, transform.position.z);
            };
            return MyDoTween.To(getter, setter, to, duration);
        }

        public static Tweener DOMoveY(this Transform transform, float to, float duration)
        {
            MyGetter<float> getter = () =>
            {
                return transform.position.y;
            };
            MySetter<float> setter = y =>
            {
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            };
            return MyDoTween.To(getter, setter, to, duration);
        }

        public static Tweener DOMoveZ(this Transform transform, float to, float duration)
        {
            MyGetter<float> getter = () =>
            {
                return transform.position.z;
            };
            MySetter<float> setter = z =>
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            };
            return MyDoTween.To(getter, setter, to, duration);
        }

        public static Tweener DORotate(this Transform transform, Vector3 to, float duration)
        {
            return MyDoTween.To(() => transform.eulerAngles, x => transform.eulerAngles = x, to, duration);
        }

        public static Tweener DORotateQuaternion(this Transform transform, Quaternion to, float duration)
        {
            return MyDoTween.To(() => transform.rotation, x => transform.rotation = x, to, duration);
        }

        public static Tweener DOLocalRotate(this Transform transform, Vector3 to, float duration)
        {
            return MyDoTween.To(() => transform.localEulerAngles, x => transform.localEulerAngles = x, to, duration);
        }

        public static Tweener DOLocalRotateQuaternion(this Transform transform, Quaternion to, float duration)
        {
            return MyDoTween.To(() => transform.localRotation, x => transform.localRotation = x, to, duration);
        }

        public static Tweener DOScale(this Transform transform, Vector3 to, float duration)
        {
            return MyDoTween.To(() => transform.localScale, x => transform.localScale = x, to, duration);
        }

        public static Tweener DOScale(this Transform transform, float to, float duration)
        {
            return transform.DOScale(transform.localScale * to, duration);
        }
    }
    public static class MaterialClassFunction
    {
        // material
        public static Tweener DOColor(this Material material, Color to, float duration)
        {
            return MyDoTween.To(() => material.color, x => material.color = x, to, duration);
        }
    }
}
