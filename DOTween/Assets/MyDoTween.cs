/*
 * 描 述：实现DoTween静态类
 * 作 者：hza 
 * 创建时间：2017/05/07 24:29:03
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.LerpFunctionSpace;
using My.DoTween.Core;
using System;

namespace My.DoTween {
    public static class MyDoTween
    {
        // 自动执行
        public static bool StartAuto = true;
        private static bool isBegin = false;

        // tweener容量
        public static int TweenerMaxSize = 100;
        // sequence容量
        public static int SequenceMaxSize = 30;

        public static void Init(bool startAuto, int tweenerSize = 100, int sequenceSize = 30)
        {
            // 如果未启动，修改变量
            if (!isBegin)
            {
                StartAuto = startAuto;
                TweenerMaxSize = tweenerSize;
                SequenceMaxSize = sequenceSize;
            }
        }

        // 创建新Sequence
        public static Sequence Sequence()
        {
            Sequence se = My.DoTween.Core.Sequence.create();
            AddCoroutineAndTween(se);

            return se;
        }

        public static object GetUniqueTweenerID()
        {
            // 当新建Tween时，不允许修改条件
            isBegin = true;
            if (TweenerMaxSize == 0)
                throw new KeyNotFoundException("Tween超出数量限制");
            TweenerMaxSize--;
            return "Tweener" + Guid.NewGuid();
        }

        public static Tween FreeTweener(Tween tween)
        {
            tween.Clear();
            tween = null;
            TweenerMaxSize++;
            return tween;
        }

        public static object GetUniqueSequenceID()
        {
            // 当新建Tween时，不允许修改条件
            isBegin = true;
            if (SequenceMaxSize == 0)
                throw new KeyNotFoundException("Sequence超出数量限制");
            SequenceMaxSize--;
            return "Sequence" + Guid.NewGuid();
        }

        public static Tween FreeSequence(Tween tween)
        {
            tween.Clear();
            tween = null;
            SequenceMaxSize++;
            return tween;
        }

        #region To重载
        public static Tweener To(MyGetter<float> getter, MySetter<float> setter, float endValue, float duration)
        {
            // 设置属性
            Tweener<float> newTweener = Tweener<float>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return LerpFunction.lerfs[(int)newTweener.lerptype](from, to, time);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<double> getter, MySetter<double> setter, double endValue, float duration)
        {
            // 设置属性
            Tweener<double> newTweener = Tweener<double>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return from + (to - from) * LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<int> getter, MySetter<int> setter, int endValue, float duration)
        {
            // 设置属性
            Tweener<int> newTweener = Tweener<int>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                // 四舍五入
                return from + (int)((to - from) * LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time) + 0.5);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Vector2> getter, MySetter<Vector2> setter, Vector2 endValue, float duration)
        {
            // 设置属性
            Tweener<Vector2> newTweener = Tweener<Vector2>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return from + (to - from) * LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Vector3> getter, MySetter<Vector3> setter, Vector3 endValue, float duration)
        {
            // 设置属性
            Tweener<Vector3> newTweener = Tweener<Vector3>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return from + (to - from) * LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Vector4> getter, MySetter<Vector4> setter, Vector4 endValue, float duration)
        {
            // 设置属性
            Tweener<Vector4> newTweener = Tweener<Vector4>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return from + (to - from) * LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time);
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Quaternion> getter, MySetter<Quaternion> setter, Quaternion endValue, float duration)
        {
            // 设置属性
            Tweener<Quaternion> newTweener = Tweener<Quaternion>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return Quaternion.Lerp(from, to, LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time));
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Rect> getter, MySetter<Rect> setter, Rect endValue, float duration)
        {
            // 设置属性
            Tweener<Rect> newTweener = Tweener<Rect>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return new Rect(LerpFunction.lerfs[(int)newTweener.lerptype](from.x, to.x, time),
                                LerpFunction.lerfs[(int)newTweener.lerptype](from.y, to.y, time),
                                LerpFunction.lerfs[(int)newTweener.lerptype](from.width, to.width, time),
                                LerpFunction.lerfs[(int)newTweener.lerptype](from.height, to.height, time));
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<RectOffset> getter, MySetter<RectOffset> setter, RectOffset endValue, float duration)
        {
            // 设置属性
            Tweener<RectOffset> newTweener = Tweener<RectOffset>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return new RectOffset((int)LerpFunction.lerfs[(int)newTweener.lerptype](from.left, to.left, time),
                                      (int)LerpFunction.lerfs[(int)newTweener.lerptype](from.right, to.right, time),
                                      (int)LerpFunction.lerfs[(int)newTweener.lerptype](from.top, to.top, time),
                                      (int)LerpFunction.lerfs[(int)newTweener.lerptype](from.bottom, to.bottom, time));
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        public static Tweener To(MyGetter<Color> getter, MySetter<Color> setter, Color endValue, float duration)
        {
            Tweener<Color> newTweener = Tweener<Color>.Create(getter, setter, endValue, duration);

            newTweener.change = (from, to, time) =>
            {
                return Color.Lerp(from, to, LerpFunction.lerfs[(int)newTweener.lerptype](0, 1, time));
            };

            // 加入Tween
            AddCoroutineAndTween(newTweener);

            return newTweener;
        }
        #endregion

        #region 调用Manager类函数
        public static void AddCoroutineAndTween(Tween tween)
        {
            MyTweenManager manager = MyTweenManager.GetInstance();
            manager.AddCoroutineAndTween(tween);
        }

        
        public static void DeleteCoroutineAndTween(Tween tween)
        {
            MyTweenManager manager = MyTweenManager.GetInstance();
            manager.DeleteCoroutineAndTween(tween);
        }

        public static List<Tween> GetAllTween()
        {
            MyTweenManager manager = MyTweenManager.GetInstance();
            return manager.GetAllTween();
        }

        public static List<Tween> GetAllPauseTween()
        {
            MyTweenManager manager = MyTweenManager.GetInstance();
            return manager.GetAllPauseTween();
        }

        public static List<Tween> GetAllPlayingTween()
        {
            MyTweenManager manager = MyTweenManager.GetInstance();
            return manager.GetAllPlayingTween();
        }
        #endregion
    }
}
