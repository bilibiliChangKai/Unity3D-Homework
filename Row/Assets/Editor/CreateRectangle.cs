/*
 * 描 述：用于创建四边形的文件，我只是大自然的搬运工
 * 作 者：hza 
 * 创建时间：2017/04/06 13:50:10
 * 版 本：v 1.0
 */

using UnityEngine;
using UnityEditor;
using System.Collections;


public class CreateRectangle : ScriptableWizard
{
    /* 属性含义：
     * width: 四边形宽度(x轴)
     * length: 四边形高度(z轴)
     * angle: 两边的夹角，默认为90°[0, 180]
     * addCollider: 是否需要添加碰撞器，默认为false
     */
    public float width = 1;
    public float length = 1;
    public float angle = 90;
    public bool addCollider = false;

    [MenuItem("GameObject/Create Other/Rectangle")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create Rectangle", typeof(CreateRectangle));
    }

    void OnWizardCreate()
    {
        GameObject newRectangle = new GameObject("Rectangle");

        // 保存mash的名字和路径
        string meshName = newRectangle.name + "w" + width + "l" + length + "a" + angle;
        string meshPrefabPath = "Assets/Editor/" + meshName + ".asset";

        // 对角度进行处理
        while (angle > 180) angle -= 180;
        while (angle < 0) angle += 180;

        angle *= Mathf.PI / 180;

        /* 1. 顶点，三角形，法线，uv坐标, 绝对必要的部分只有顶点和三角形。 
         * 如果模型中不需要场景中的光照，那么就不需要法线。
         * 如果模型不需要贴材质，那么就不需要UV 
         */
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        vertices[0] = new Vector3(0, 0, 0);
        uv[0] = new Vector2(0, 0);
        normals[0] = Vector3.up;

        vertices[1] = new Vector3(0, 0, length);
        uv[1] = new Vector2(0, 1);
        normals[1] = Vector3.up;


        vertices[2] = new Vector3(width * Mathf.Sin(angle), 0, length + width * Mathf.Cos(angle));
        uv[2] = new Vector2(1, 1);
        normals[2] = Vector3.up;

        vertices[3] = new Vector3(width * Mathf.Sin(angle), 0, width * Mathf.Cos(angle));
        uv[3] = new Vector2(1, 0);
        normals[3] = Vector3.up;

        /* 2. 三角形,顶点索引： 
		 * 三角形是由3个整数确定的，各个整数就是角的顶点的index。
         * 各个三角形的顶点的顺序通常由下往上数， 可以是顺时针也可以是逆时针，这通常取决于我们从哪个方向看三角形。
         * 通常，当mesh渲染时，"逆时针" 的面会被挡掉。 我们希望保证顺时针的面与法线的主向一致 
         */
        int[] indices = new int[6];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;

        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;

        Mesh mesh = new Mesh();
        mesh.name = meshName;
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
        AssetDatabase.CreateAsset(mesh, meshPrefabPath);
        AssetDatabase.SaveAssets();

        // 添加MeshFilter
        MeshFilter filter = newRectangle.gameObject.AddComponent<MeshFilter>();
        if (filter != null)
        {
            filter.sharedMesh = mesh;
        }

        // 添加MeshRendered
        MeshRenderer meshRender = newRectangle.gameObject.AddComponent<MeshRenderer>();
        Shader shader = Shader.Find("Standard");
        meshRender.sharedMaterial = new Material(shader);

        // 如果愿意添加碰撞器，则添加碰撞器
        if (addCollider)
        {
            MeshCollider mc = newRectangle.AddComponent<MeshCollider>();
            mc.sharedMesh = filter.sharedMesh;
        }

        Selection.activeObject = newRectangle;
    }
}

