using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunrun : MonoBehaviour {

    public Transform sun;
    public Transform mercury;
    public Transform venus;
    public Transform earth;
    public Transform mars;
    public Transform jupiter;
    public Transform saturn;
    public Transform uranus;
    public Transform nepturn;
    public Transform pluto;

    private Vector3[] selfAxis; // 自转法向量数组
    private Vector3[] sunAxis; // 公转法向量数组
    private float[] selfRotateTime; // 自转周期时间
    private float[] sunRotateTime; // 公转周期时间

    private List<Transform> planetList; // 星球链表

    private void resetTime()
    {
        selfRotateTime = new float[] {
            300,
            100,
            359,
            100,
            120,
            1000,
            150,
            200,
            30
        };
        sunRotateTime = new float[] {
            60,
            30,
            20,
            16,
            12,
            4,
            2,
            1,
            0.4f
        };
    }

    private void resetVector()
    {
        int size = planetList.Count;
        sunAxis = new Vector3[size];
        selfAxis = new Vector3[size];
        for (int i = 0; i < size; i++)
            selfAxis[i] = sunAxis[i] = Vector3.Cross(planetList[i].position - sun.position, Vector3.forward);
        // 公转的法平面和一般行星自转的法平面都是位置与（0,0,1）的叉积

        selfAxis[1] = -selfAxis[1]; // 金星自传方向相反
        selfAxis[7] = planetList[7].position - sun.position; // 天王星自转和公转方向90°
    }
    // Use this for initialization
    private void Awake()
    {
        Transform[] temp = { mercury, venus, earth, mars, jupiter, saturn, uranus, nepturn, pluto };
        planetList = new List<Transform>();
        planetList.AddRange(temp); // list初始化
        resetTime();
        resetVector();
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < planetList.Count; i++)
        {
            planetList[i].Rotate(selfAxis[i], selfRotateTime[i] * Time.deltaTime); 
            // 设置每个行星的自传
            planetList[i].RotateAround(sun.position, sunAxis[i], sunRotateTime[i] * Time.deltaTime);
            // 设置每个行星的公转
        }
	}
}
