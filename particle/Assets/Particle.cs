/*
 * 描 述：用于描述粒子运动
 * 作 者：hza 
 * 创建时间：2017/05/01 13:04:58
 * 版 本：v 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleParticle
{
    public float r = 0;  // 半径
    public float a = 0;  // 角度
    public float t = 0;  // 时间
    public CircleParticle(float _r, float _a, float _t) {
        r = _r;
        a = _a;
        t = _t;
    }
}

public class Particle : MonoBehaviour {

    private ParticleSystem particleSys;
    private ParticleSystem.Particle[] particleArr;
    private CircleParticle[] circleParticle;
    public Gradient colorGradient; // 透明度


    public int count = 10000; // 数量
    public float size = 0.03f; // 大小
    public float maxRadius = 12f; // 最大半径
    public float minRadius = 5f; // 最小半径
    public bool clockwise = true; // 顺时针或逆时针
    public float speed = 2f; // 速度
    public float pingPong = 0.02f;  // 游离范围

    // Use this for initialization
    void Start () {
        // 初始化粒子数组  
        particleArr = new ParticleSystem.Particle[count];
        circleParticle = new CircleParticle[count];

        // 初始化粒子系统  
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.startSpeed = 0;            // 粒子位置由程序控制  
        particleSys.startSize = size;          // 设置粒子大小  
        particleSys.loop = false;
        particleSys.maxParticles = count;      // 设置最大粒子量  
        particleSys.Emit(count);               // 发射粒子  
        particleSys.GetParticles(particleArr);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f;
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f;
        alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f;
        alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f;
        alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f;
        alphaKeys[4].alpha = 0.9f;
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0.0f;
        colorKeys[0].color = Color.white;
        colorKeys[1].time = 1.0f;
        colorKeys[1].color = Color.white;
        colorGradient.SetKeys(colorKeys, alphaKeys);

        RandomlySpread();   // 初始化各粒子位置  
    }

    private int tier = 10;  // 速度差分层数  
    void Update () {
        for (int i = 0; i < count; i++)
        {
            if (clockwise) circleParticle[i].a -= (i % tier + 1) * (speed / circleParticle[i].r / tier); // 顺时针旋转
            else circleParticle[i].a += (i % tier + 1) * (speed / circleParticle[i].r / tier); // 逆时针旋转 

            // 保证angle在0~360度  
            circleParticle[i].a = (360.0f + circleParticle[i].a) % 360.0f;
            float theta = circleParticle[i].a / 180 * Mathf.PI;

            // pingpong
            circleParticle[i].t += Time.deltaTime;
            circleParticle[i].r += Mathf.PingPong(circleParticle[i].t, pingPong) - pingPong / 2.0f;

            // 设置透明度
            particleArr[i].color = colorGradient.Evaluate(circleParticle[i].a / 360.0f);

            particleArr[i].position = new Vector3(circleParticle[i].r * Mathf.Cos(theta), 0, circleParticle[i].r * Mathf.Sin(theta));
        }

        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    void RandomlySpread()
    {
        for (int i = 0; i < count; i++)
        {   
            // 随机每个粒子距离中心的半径，同时希望粒子集中在平均半径附近  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            // 随机每个粒子的角度  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;

            // 随机每个粒子的游离起始时间  
            float time = Random.Range(0.0f, 360.0f);

            circleParticle[i] = new CircleParticle(radius, angle, time);

            particleArr[i].position = new Vector3(circleParticle[i].r * Mathf.Cos(theta), 0f, circleParticle[i].r * Mathf.Sin(theta));
        }

        particleSys.SetParticles(particleArr, particleArr.Length);
    }
}
