using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GraphMgr
{
    public static GraphMgr Instance;

    public int[][] graph;

    public void init()
    {
        Instance = this;
        this.scanGraph();
    }

    public void scanGraph()
    {
        Transform[] objs = MouseMgr.instance.graphAnchor.transform.GetComponentsInChildren<Transform>(true);
        // -1 是地图x轴格子数, +2 是x轴两边的边界, 这样写以便理解
        this.graph = new int[MouseMgr.instance.rangeX.maxX - 1 + 2][];
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i] = new int[MouseMgr.instance.rangeZ.maxZ - 1 + 2];
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

    public int getVal(int x, int z)
    {
        return this.checkRange(x, z) ? this.graph[x][z] : -1;
    }

    public void setVal(int x, int z, int val)
    {
        if(this.checkRange(x, z))
        {
            this.graph[x][z] = val;
        }
    }

    private bool checkRange(int x, int z)
    {
        if (x < 0 || x > this.graph.Length-1 || z < 0 || z > this.graph[0].Length-1) return false;
        return true;
    }

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
