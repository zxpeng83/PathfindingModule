using GameConfig;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// 地图管理单例
/// </summary>
public class GraphMgr: MonoBehaviour
{
    public static GraphMgr Instance;

    /// <summary>
    /// 地图抽象到代码的障碍物图 1为障碍物 0为平地
    /// </summary>
    private int[][] graph;
    /// <summary>
    /// 地图坐标0点,同时也是障碍物容器
    /// </summary>
    public GameObject graphAnchor;
    /// <summary>
    /// 需要放置障碍物时跟随鼠标移动的预瞄障碍物
    /// </summary>
    private GameObject fakeBarrier = null;
    /// <summary>
    /// 需要放置目标点时跟随鼠标移动的预瞄目标点
    /// </summary>
    private GameObject fakeTarget = null;
    /// <summary>
    /// 地图上要放置物体时的预瞄物体
    /// </summary>
    private GameObject fakeObj = null;

    //场景中地图x z轴的最大最小值
    public (int minX, int maxX) rangeX = (minX: 1, maxX: 21);
    public (int minZ, int maxZ) rangeZ = (minZ: 1, maxZ: 21);

    private void Awake()
    {
        Instance = this;
        this.scanGraph();
    }

    /// <summary>
    /// 扫描地图建图
    /// </summary>
    public void scanGraph()
    {
        Transform[] objs =this.graphAnchor.transform.GetComponentsInChildren<Transform>(true);
        // -1 是地图x轴格子数, +2 是x轴两边的边界, 这样写以便理解
        this.graph = new int[this.rangeX.maxX - 1 + 2][];
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i] = new int[this.rangeZ.maxZ - 1 + 2];
        }

        //初始化围墙
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i][0] = this.graph[i][this.graph[0].Length-1] = 1;
        }
        for(int i = 0; i < this.graph[0].Length; i++)
        {
            this.graph[0][i] = this.graph[this.graph.Length-1][i] = 1;
        }

        //障碍物建图
        foreach (var item in objs)
        {
            string[] str = item.gameObject.name.Split("_");
            if (str.Length < 3) return;

            int xx = int.Parse(str[1]);
            int zz = int.Parse(str[2]);

            this.graph[xx][zz] = 1;
        }
    }

    public void putFakeObj(Vector3 pos, FakeObjType type)
    {
        if(type == FakeObjType.Barrier)
        {

        }
    }

    /// <summary>
    /// 获取地图上该点的权值(1为障碍物,0为平地)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int getVal(int x, int z)
    {
        return this.checkRange(x, z) ? this.graph[x][z] : -1;
    }

    /// <summary>
    /// 设置地图上某点的值
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="val"></param>
    public void setVal(int x, int z, int val)
    {
        if(this.checkRange(x, z))
        {
            this.graph[x][z] = val;
        }
    }



    /// <summary>
    /// 判断该点是否在地图范围内
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private bool checkRange(int x, int z)
    {
        if (x < 0 || x > this.graph.Length-1 || z < 0 || z > this.graph[0].Length-1) return false;
        return true;
    }

    //TODO debug用的打印,可删
    public void prin()
    {
        string str = "";
        for(int i = this.graph[0].Length - 1; i >= 0; i--)
        {
            for(int j = 0; j <= this.graph.Length - 1; j++)
            {
                str += this.graph[j][i];
            }
            str += "\n";
        }
        Debug.Log(str);
    }
}
