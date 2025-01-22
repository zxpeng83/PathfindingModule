using GameConfig;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// ��ͼ������
/// </summary>
public class GraphMgr: MonoBehaviour
{
    public static GraphMgr Instance;

    /// <summary>
    /// ��ͼ���󵽴�����ϰ���ͼ 1Ϊ�ϰ��� 0Ϊƽ��
    /// </summary>
    private int[][] graph;
    /// <summary>
    /// ��ͼ����0��,ͬʱҲ���ϰ�������
    /// </summary>
    public GameObject graphAnchor;
    /// <summary>
    /// ��Ҫ�����ϰ���ʱ��������ƶ���Ԥ���ϰ���
    /// </summary>
    private GameObject fakeBarrier = null;
    /// <summary>
    /// ��Ҫ����Ŀ���ʱ��������ƶ���Ԥ��Ŀ���
    /// </summary>
    private GameObject fakeTarget = null;
    /// <summary>
    /// ��ͼ��Ҫ��������ʱ��Ԥ������
    /// </summary>
    private GameObject fakeObj = null;

    //�����е�ͼx z��������Сֵ
    public (int minX, int maxX) rangeX = (minX: 1, maxX: 21);
    public (int minZ, int maxZ) rangeZ = (minZ: 1, maxZ: 21);

    private void Awake()
    {
        Instance = this;
        this.scanGraph();
    }

    /// <summary>
    /// ɨ���ͼ��ͼ
    /// </summary>
    public void scanGraph()
    {
        Transform[] objs =this.graphAnchor.transform.GetComponentsInChildren<Transform>(true);
        // -1 �ǵ�ͼx�������, +2 ��x�����ߵı߽�, ����д�Ա����
        this.graph = new int[this.rangeX.maxX - 1 + 2][];
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i] = new int[this.rangeZ.maxZ - 1 + 2];
        }

        //��ʼ��Χǽ
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i][0] = this.graph[i][this.graph[0].Length-1] = 1;
        }
        for(int i = 0; i < this.graph[0].Length; i++)
        {
            this.graph[0][i] = this.graph[this.graph.Length-1][i] = 1;
        }

        //�ϰ��ｨͼ
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
    /// ��ȡ��ͼ�ϸõ��Ȩֵ(1Ϊ�ϰ���,0Ϊƽ��)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int getVal(int x, int z)
    {
        return this.checkRange(x, z) ? this.graph[x][z] : -1;
    }

    /// <summary>
    /// ���õ�ͼ��ĳ���ֵ
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
    /// �жϸõ��Ƿ��ڵ�ͼ��Χ��
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private bool checkRange(int x, int z)
    {
        if (x < 0 || x > this.graph.Length-1 || z < 0 || z > this.graph[0].Length-1) return false;
        return true;
    }

    //TODO debug�õĴ�ӡ,��ɾ
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
