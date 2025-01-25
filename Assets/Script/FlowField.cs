using GameConfig;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowField
{
    private static FlowField ins = null;
    public static FlowField instance
    {
        get
        {
            if (ins == null)
            {
                ins = new FlowField();
            }

            return ins;
        }
    }

    private int[][] flowFieldGraph = null;

    public int[][] getGraph()
    {
        if (flowFieldGraph == null)
        {
            this.startNavigation();
        }

        return flowFieldGraph;
    }

    public void startNavigation()
    {
        if (CharMgr.charList.Count <= 0) return;

        ///地图
        int[][] graph = GraphMgr.Instance.getGraph();
        Queue<(int x, int z, int step)> que = new Queue<(int x, int z, int step)>();
        
        ///终点
        Vector2Int target = Vector2Int.zero;

        for (int i = 0; i < graph.Length; i++)
        {
            for (int j = 0; j < graph[i].Length; j++)
            {
                if (graph[i][j] == (int)GraphObjType.Target)
                {
                    target = new Vector2Int(i, j);
                }
            }
        }

        if (target == Vector2Int.zero)
        {
            GraphMgr.Instance.prin();
            Debug.LogError("FlowField寻路失败,地图中找不到终点");
            return;
        }

        this.resetFFGraph();

        que.Enqueue((target.x, target.y, 0));

        ///开始FlowField寻路
        while (que.Count > 0)
        {
            var top = que.Dequeue();

            int x = top.x;
            int z = top.z;
            int step = top.step;

            for(int i = 0; i < MoveDirec.dx.Length; i++)
            {
                int xx = x + MoveDirec.dx[i];
                int zz = z + MoveDirec.dy[i];

                //已经搜索过
                if (this.flowFieldGraph[xx][zz] >= 0) return;

                var type = GraphMgr.Instance.getVal(xx, zz);
                if (!(type == (int)GraphObjType.None || type == (int)GraphObjType.Char)) return;

                ///此时格子是 None || Char

                this.flowFieldGraph[xx][zz] = (i + 4) % 8;

                que.Enqueue((xx, zz, step + 1));
            }
        }

    }

    private void resetFFGraph()
    {
        this.flowFieldGraph = new int[GraphMgr.Instance.getGraph().Length][];
        for(int i = 0; i < GraphMgr.Instance.getGraph().Length; i++)
        {
            this.flowFieldGraph[i] = new int[GraphMgr.Instance.getGraph()[i].Length];
        }

        for(int i = 0; i < this.flowFieldGraph.Length; i++)
        {
            for(int j = 0; j < this.flowFieldGraph[i].Length; j++)
            {
                this.flowFieldGraph[i][j] = -1;
            }
        }
    }
}
