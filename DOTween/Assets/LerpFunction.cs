/*
 * 描 述：存放各种插值函数的类
 * 作 者：hza 
 * 创建时间：2017/05/07 00:48:02
 * 版 本：v 1.0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.LerpFunctionSpace
{
    // Lerp函数类型
    public enum LerpFunctionType : int
    {
        Linear = 0,
        QuadIn = 1,
        QuadOut = 2,
        QuadBoth = 3,
        CubicIn = 4,
        CubicOut = 5,
        CubicBoth = 6,
        QuintIn = 7,
        QuintOut = 8,
        QuintBoth = 9,
        SineIn = 10,
        SineOut = 11,
        SineBoth = 12,
        ExpoIn = 13,
        ExpoOut = 14,
        ExpoBoth = 15,
        CircIn = 16,
        CircOut = 17,
        CircBoth = 18,
        ElasticIn = 19,
        ElasticOut = 20,
        ElasticBoth = 21,
        BackIn = 22,
        BackOut = 23,
        BackBoth = 24,
        BounceIn = 25,
        BounceOut = 26,
        BounceBoth = 27
    }

    /// <summary>
    /// 用来进行各种线性插值的计算，time必须在0-1之间
    /// </summary>
    public static class LerpFunction
    {
        #region 委托和事件数组
        public delegate float LFunction(float from, float to, float time);
        public static LFunction[] lerfs = new LFunction[]
        {
            Linear,
            QuadIn,
            QuadOut,
            QuadBoth,
            CubicIn,
            CubicOut,
            CubicBoth,
            QuintIn,
            QuintOut,
            QuintBoth,
            SineIn,
            SineOut,
            SineBoth,
            ExpoIn,
            ExpoOut,
            ExpoBoth,
            CircIn,
            CircOut,
            CircBoth,
            ElasticIn,
            ElasticOut,
            ElasticBoth,
            BackIn,
            BackOut,
            BackBoth,
            BounceIn,
            BounceOut,
            BounceBoth
        };
        #endregion

        #region 私有Lerp函数
        // 线性
        public static float Linear(float from, float to, float time)
        {
            return from + (to - from) * time;
        }

        // 2次方程
        public static float QuadIn(float from, float to, float time)
        {
            return from + (to - from) * time * time;
        }
        public static float QuadOut(float from, float to, float time)
        {
            return from + (to - from) * time * (2f - time);
        }
        public static float QuadBoth(float from, float to, float time)
        {
            return time < 0.5f ? QuadIn(from, (from + to) / 2, time * 2) : QuadOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 3次方程
        public static float CubicIn(float from, float to, float time)
        {
            return from + (to - from) * Mathf.Pow(time, 3f);
        }
        public static float CubicOut(float from, float to, float time)
        {
            return from + (to - from) * (Mathf.Pow(time - 1, 3f) + 1.0f);
        }
        public static float CubicBoth(float from, float to, float time)
        {
            return time < 0.5f ? CubicIn(from, (from + to) / 2, time * 2) : CubicOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 5次方程
        public static float QuintIn(float from, float to, float time)
        {
            return from + (to - from) * Mathf.Pow(time, 5f);
        }
        public static float QuintOut(float from, float to, float time)
        {
            return from + (to - from) * (Mathf.Pow(time - 1, 5f) + 1.0f);
        }
        public static float QuintBoth(float from, float to, float time)
        {
            return time < 0.5f ? QuintIn(from, (from + to) / 2, time * 2) : QuintOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 正弦曲线
        public static float SineIn(float from, float to, float time)
        {
            return from + (to - from) * (1.0f - Mathf.Cos(time * Mathf.PI / 2));
        }
        public static float SineOut(float from, float to, float time)
        {
            return from + (to - from) * Mathf.Sin(time * Mathf.PI / 2);
        }
        public static float SineBoth(float from, float to, float time)
        {
            return time < 0.5f ? SineIn(from, (from + to) / 2, time * 2) : SineOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 指数增长
        public static float ExpoIn(float from, float to, float time)
        {
            return time == 0.0f ? from : from + (to - from) * Mathf.Pow(2f, 10f * (time - 1f));
        }

        public static float ExpoOut(float from, float to, float time)
        {
            return time == 1.0f ? to : from + (to - from) * (1f - Mathf.Pow(2f, -10f * time));
        }

        public static float ExpoBoth(float from, float to, float time)
        {
            return time < 0.5f ? ExpoIn(from, (from + to) / 2, time * 2) : ExpoOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 圆周弧线
        static float CircIn(float from, float to, float time)
        {
            return from + (to - from) * (1.0f - Mathf.Sqrt(1f - time * time));
        }
        static float CircOut(float from, float to, float time)
        {
            return from + (to - from) * Mathf.Sqrt((2f - time) * time);
        }
        static float CircBoth(float from, float to, float time)
        {
            return time < 0.5f ? CircIn(from, (from + to) / 2, time * 2) : CircOut((from + to) / 2, to, 2 * time - 1f);
        }

        // 弹性弧线
        public static float ElasticIn(float from, float to, float time)
        {
            if (time == 0.0f) return from;
            if (time == 1.0f) return to;
            return from + (to - from) * -Mathf.Pow(2.0f, 10.0f * time - 10.0f) * Mathf.Sin((3.33f * time - 3.58f) * Mathf.PI * 2);
        }
        public static float ElasticOut(float from, float to, float time)
        {
            if (time == 0.0f) return from;
            if (time == 1.0f) return to;
            return from + (to - from) * (Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((3.33f * time - 0.25f) * Mathf.PI * 2) + 1.0f);
        }
        public static float ElasticBoth(float from, float to, float time)
        {
            return time < 0.5f ? ElasticIn(from, (from + to) / 2, time * 2) : ElasticOut((from + to) / 2, to, 2 * time - 1f);
        }

        // back曲线
        public static float BackIn(float from, float to, float time)
        {
            return from + (to - from) * time * time * (2.70158f * time - 1.70158f);
        }
        public static float BackOut(float from, float to, float time)
        {
            return from + (to - from) * (Mathf.Pow(time - 1, 2f) * (2.70158f * time - 1f) + 1.0f);
        }
        public static float BackBoth(float from, float to, float time)
        {
            return time < 0.5f ? BackIn(from, (from + to) / 2, time * 2) : BackOut((from + to) / 2, to, 2 * time - 1f);
        }
        
        // 弹性曲线
        public static float BounceOut(float from, float to, float time)
        {
            if (time < 0.363636f) return from + (to - from) * 7.5625f * time * time;
            else if (time < 0.72727f)
            {
                time -= 0.545454f;
                return from + (to - from) * (7.5625f * time * time + 0.75f);
            }
            else if (time < 0.909091f)
            {
                time -= 0.818182f;
                return from + (to - from) * (7.5625f * time * time + 0.9375f);
            }
            else
            {
                time -= 0.954545f;
                return from + (to - from) * (7.5625f * time * time + 0.984375f);
            }
        }
        public static float BounceIn(float from, float to, float time)
        {
            if (time > 0.636364f)
            {
                time = 1.0f - time;
                return from + (to - from) * (1.0f - 7.5625f * time * time);
            }
            else if (time > 0.27273f)
            {
                time = 0.454546f - time;
                return from + (to - from) * (0.25f - 7.5625f * time * time);
            }
            else if (time > 0.090909f)
            {
                time = 0.181818f - time;
                return from + (to - from) * (0.0625f - 7.5625f * time * time);
            }
            else
            {
                time = 0.045455f - time;
                return from + (to - from) * (0.015625f - 7.5625f * time * time);
            }
        }
        public static float BounceBoth(float from, float to, float time)
        {
            return time < 0.5f ? BounceIn(from, (from + to) / 2, time * 2) : BounceOut((from + to) / 2, to, 2 * time - 1f);
        }
        #endregion
    }
}
