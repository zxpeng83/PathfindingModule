using GameConfig;
using System;
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
    private (int x, int z) target;
    /// <summary>
    /// 地图坐标0点,同时也是障碍物容器
    /// </summary>
    public GameObject graphAnchor;
    ///// <summary>
    ///// 需要放置障碍物时跟随鼠标移动的预瞄障碍物
    ///// </summary>
    //private GameObject fakeBarrier = null;
    ///// <summary>
    ///// 需要放置目标点时跟随鼠标移动的预瞄目标点
    ///// </summary>
    //private GameObject fakeTarget = null;
    /// <summary>
    /// 地图上要放置物体时的预瞄物体
    /// </summary>
    private GameObject fakeObj = null;

    ////场景中地图x z轴的最大最小值
    //public (int minX, int maxX) rangeX = (minX: 1, maxX: 21);
    //public (int minZ, int maxZ) rangeZ = (minZ: 1, maxZ: 21);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.buildAndScanGraph();
    }

    public int[][] getGraph()
    {
        return graph;
    }

    /// <summary>
    /// 建图并扫描地图
    /// </summary>
    public void buildAndScanGraph()
    {
        // -1 是地图x轴格子数, +2 是x轴两边的边界, 这样写以便理解
        this.graph = new int[GameConfig.RangeXYZ.maxX - 1 + 2][];
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i] = new int[GameConfig.RangeXYZ.maxZ - 1 + 2];
        }

        //初始化围墙
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i][0] = this.graph[i][this.graph[0].Length-1] = (int)GraphObjType.Wall;
        }
        for(int i = 0; i < this.graph[0].Length; i++)
        {
            this.graph[0][i] = this.graph[this.graph.Length-1][i] = (int)GraphObjType.Wall;
        }

        this.scanGraph();
    }

    /// <summary>
    /// 扫描并刷新地图
    /// </summary>
    public void scanGraph()
    {
        GameObject[] objs = ObjTool.instance.getChildWithL1(this.graphAnchor);
        //扫描动态物体
        foreach (var item in objs)
        {
            Debug.LogError(item.name);
            string[] str = item.name.Split("-");
            if (str.Length < 3) continue;

            int xx = int.Parse(str[1]);
            int zz = int.Parse(str[2]);
            string type = str[0];

            if (!Enum.TryParse(type, out GraphObjType eType)) continue;

            // 需要记录进地图的物体:  目标                          障碍物                  角色                            预瞄物体
            if(eType == GraphObjType.Target || eType == GraphObjType.Barrier || eType == GraphObjType.Char || eType == GraphObjType.Fake)
            {
                this.graph[xx][zz] = (int)eType;
            }
        }
    }

    /// <summary>
    /// 在地图上放置预瞄物体
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    public void putFakeObj(Vector3 pos, GraphObjType type)
    {
        {
            //Debug.Log("放置预瞄物体");
            //if(type == GraphObjType.Barrier)
            //{
            //    if (!this.fakeBarrier)
            //    {
            //        this.fakeBarrier = ObjPool.instance.getObj(type.ToString());
            //    }
            //    ObjTool.instance.setLayWithChild(this.fakeBarrier, 2);
            //    this.fakeBarrier.transform.parent = this.graphAnchor.transform;
            //    this.fakeBarrier.transform.localPosition = pos;
            //}
            //else if(type == GraphObjType.Target)
            //{
            //    if (!this.fakeTarget)
            //    {
            //        this.fakeTarget = ObjPool.instance.getObj(type.ToString());
            //    }
            //    ObjTool.instance.setLayWithChild(this.fakeTarget, 2);
            //    this.fakeTarget.transform.parent = this.graphAnchor.transform;
            //    this.fakeTarget.transform.localPosition= pos;

            //}
        }

        if(type == GraphObjType.Fake)
        {
            if (!this.fakeObj)
            {
                this.fakeObj = ObjPool.instance.getObj(type.ToString());
            }
            ObjTool.instance.setLayWithChild(this.fakeObj, 2);
            this.fakeObj.transform.parent = this.graphAnchor.transform;
            this.fakeObj.transform.localPosition = pos;
            this.fakeObj.name = type.ToString() + "-" + Mathf.FloorToInt(pos.x) + "-" + Mathf.FloorToInt(pos.z);
        }

        {
            //if (type != GraphObjType.Target && this.fakeTarget)
            //{
            //    ObjTool.instance.setLayWithChild(this.fakeTarget, 0);
            //    ObjPool.instance.backObj(this.fakeTarget);
            //    this.fakeTarget = null;
            //}
            //if (type != GraphObjType.Barrier && this.fakeBarrier)
            //{
            //    ObjTool.instance.setLayWithChild(this.fakeBarrier, 0);
            //    ObjPool.instance.backObj(this.fakeBarrier);
            //    this.fakeBarrier = null;
            //}
            //if(type != GraphObjType.Fake && this.fakeObj)
            //{
            //    ObjTool.instance.setLayWithChild(this.fakeObj, 0);
            //    ObjPool.instance.backObj(this.fakeObj);
            //    this.fakeObj = null;
            //}
        }
    }

    /// <summary>
    /// 清除所有预瞄物体
    /// </summary>
    public void clearAllFakeObj()
    {
        Debug.Log("清除所有预瞄物体");
        if (this.fakeObj)
        {
            ObjTool.instance.setLayWithChild(this.fakeObj, 0);
            ObjPool.instance.backObj(this.fakeObj);
            this.fakeObj = null;
        }

        {
            //if (this.fakeBarrier)
            //{
            //    ObjTool.instance.setLayWithChild(this.fakeBarrier, 0);
            //    ObjPool.instance.backObj(this.fakeBarrier);
            //    this.fakeBarrier = null;
            //}
            //if (this.fakeTarget)
            //{
            //    ObjTool.instance.setLayWithChild(this.fakeTarget, 0);
            //    ObjPool.instance.backObj(this.fakeTarget);
            //    this.fakeTarget = null;
            //}
        }
    }

    /// <summary>
    /// 在地图上放置目标或障碍物物体
    /// </summary>
    /// <param name="pos">地图的格子坐标</param>
    /// <param name="type"></param>
    public void putTarOrBarObj(Vector3 pos, GraphObjType type)
    {
        if (type != GraphObjType.Target && type != GraphObjType.Barrier) return;

        GameObject obj = ObjPool.instance.getObj(type.ToString());

        if (obj)
        {
            obj.transform.parent = this.graphAnchor.transform;
            ObjTool.instance.setLayWithChild(obj, 0);
            obj.transform.localPosition = pos;
            string Newname = obj.name + "-" + Mathf.FloorToInt(pos.x) + "-" + Mathf.FloorToInt(pos.z);
            ObjTool.instance.setNameWithChild(obj, Newname);

            this.setVal(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z), (int)type);
        }
        else
        {
            Debug.LogError("生成物体错误");
        }
    }

    /// <summary>
    /// 移除已经放置的目标或障碍物物体
    /// </summary>
    /// <param name="type">要清除的类型,全部清除</param>
    public void removeTarOrBarObj(GraphObjType type)
    {
        if (type != GraphObjType.Target && type != GraphObjType.Barrier) return;

        GameObject[] gos = new GameObject[this.graphAnchor.transform.childCount];

        for(int i = 0; i < this.graphAnchor.transform.childCount; i++)
        {
            GameObject child = this.graphAnchor.transform.GetChild(i).gameObject;
            gos[i] = child;
        }

        for(int i=0;i<gos.Length;i++)
        {
            GameObject child = gos[i];
            if(!child) continue;

            if(child.name.IndexOf(type.ToString()) != -1){
                string[] tem = child.name.Split("-");
                if (tem.Length != 3) continue;

                int xx = int.Parse(tem[1]);
                int zz = int.Parse(tem[2]);
                this.graph[xx][zz] = (int)GraphObjType.None;

                ObjPool.instance.backObj(child);
            }
        }
    }

    /// <summary>
    /// 刷新角色在地图上的位置
    /// </summary>
    /// <param name="pos"></param>
    public void freshChar(Vector3 pos)
    {
        if (this.graph[(int)pos.x][(int)pos.z] == (int)GraphObjType.None || this.graph[(int)pos.x][(int)pos.z] == (int)GraphObjType.Char)
        {
            this.graph[(int)pos.x][(int)pos.z] = (int)GraphObjType.Char;
        }
        else
        {
            Debug.LogError("可能穿模了");
        }
    }

    /// <summary>
    /// 移除地图上角色的标记
    /// </summary>
    /// <param name="pos"></param>
    public void removeChar(Vector3 pos)
    {
        if(this.graph[(int)pos.x][(int)pos.z] == (int)GraphObjType.Char)
        {
            this.graph[(int)pos.x][(int)pos.z] = (int)GraphObjType.None;
        }
    }

    /// <summary>
    /// 将世界坐标转化为地图上的本地坐标(即this.graphAnchor的本地坐标)
    /// </summary>
    /// <param name="pos">世界坐标</param>
    /// <returns>
    /// flag:是否在地图范围内
    /// pos:是的话其在地图上的本地坐标
    /// </returns>
    public (bool flag, Vector3 pos) worldPos2AnchorLocalPos(Vector3 pos)
    {
        Vector3 rtnPos = graphAnchor.transform.InverseTransformPoint(pos);
        bool rtnFlag = this.checkPosInGraph(rtnPos);

        if (rtnFlag)
        {
            rtnPos.y = 0;
        }

        return (flag: rtnFlag, pos: rtnPos);
    }

    /// <summary>
    /// 世界坐标转地图的格子坐标(格子内取格子中心坐标)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public (bool flag, Vector3 pos) worldPos2AnchorLocalCenterPos(Vector3 pos)
    {
        var rtn = this.worldPos2AnchorLocalPos(pos);
        rtn.pos = new Vector3(Mathf.Floor(rtn.pos.x) + 0.5f, rtn.pos.y, Mathf.Floor(rtn.pos.z) + 0.5f);

        return rtn; 
    }

    /// <summary>
    /// 世界坐标转地图格子的下标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public (bool flag, Vector3 pos) worldPos2GraphIdx(Vector3 pos)
    {
        var rtn = this.worldPos2AnchorLocalPos(pos);
        rtn.pos = new Vector3(Mathf.FloorToInt(rtn.pos.x), rtn.pos.y, Mathf.FloorToInt(rtn.pos.z));

        return rtn;
    }

    /// <summary>
    /// 检查坐标是否在地图内
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool checkPosInGraph(Vector3 pos)
    {
        if (pos.x <= GameConfig.RangeXYZ.minX || pos.x >= GameConfig.RangeXYZ.maxX
            || pos.z <= GameConfig.RangeXYZ.minZ || pos.z >= GameConfig.RangeXYZ.maxZ
            || pos.y <= -0.15 || pos.y >= 0.15)
        {
            return false;
        }

        return true;
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
        Debug.LogError(str);
    }
}
