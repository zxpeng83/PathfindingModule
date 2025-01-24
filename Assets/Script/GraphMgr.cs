using GameConfig;
using System;
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
    private (int x, int z) target;
    /// <summary>
    /// ��ͼ����0��,ͬʱҲ���ϰ�������
    /// </summary>
    public GameObject graphAnchor;
    ///// <summary>
    ///// ��Ҫ�����ϰ���ʱ��������ƶ���Ԥ���ϰ���
    ///// </summary>
    //private GameObject fakeBarrier = null;
    ///// <summary>
    ///// ��Ҫ����Ŀ���ʱ��������ƶ���Ԥ��Ŀ���
    ///// </summary>
    //private GameObject fakeTarget = null;
    /// <summary>
    /// ��ͼ��Ҫ��������ʱ��Ԥ������
    /// </summary>
    private GameObject fakeObj = null;

    ////�����е�ͼx z��������Сֵ
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
    /// ��ͼ��ɨ���ͼ
    /// </summary>
    public void buildAndScanGraph()
    {
        // -1 �ǵ�ͼx�������, +2 ��x�����ߵı߽�, ����д�Ա����
        this.graph = new int[GameConfig.RangeXYZ.maxX - 1 + 2][];
        for(int i = 0; i < this.graph.Length; i++)
        {
            this.graph[i] = new int[GameConfig.RangeXYZ.maxZ - 1 + 2];
        }

        //��ʼ��Χǽ
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
    /// ɨ�貢ˢ�µ�ͼ
    /// </summary>
    public void scanGraph()
    {
        GameObject[] objs = ObjTool.instance.getChildWithL1(this.graphAnchor);
        //ɨ�趯̬����
        foreach (var item in objs)
        {
            Debug.LogError(item.name);
            string[] str = item.name.Split("-");
            if (str.Length < 3) continue;

            int xx = int.Parse(str[1]);
            int zz = int.Parse(str[2]);
            string type = str[0];

            if (!Enum.TryParse(type, out GraphObjType eType)) continue;

            // ��Ҫ��¼����ͼ������:  Ŀ��                          �ϰ���                  ��ɫ                            Ԥ������
            if(eType == GraphObjType.Target || eType == GraphObjType.Barrier || eType == GraphObjType.Char || eType == GraphObjType.Fake)
            {
                this.graph[xx][zz] = (int)eType;
            }
        }
    }

    /// <summary>
    /// �ڵ�ͼ�Ϸ���Ԥ������
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="type"></param>
    public void putFakeObj(Vector3 pos, GraphObjType type)
    {
        {
            //Debug.Log("����Ԥ������");
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
    /// �������Ԥ������
    /// </summary>
    public void clearAllFakeObj()
    {
        Debug.Log("�������Ԥ������");
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
    /// �ڵ�ͼ�Ϸ���Ŀ����ϰ�������
    /// </summary>
    /// <param name="pos">��ͼ�ĸ�������</param>
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
            Debug.LogError("�����������");
        }
    }

    /// <summary>
    /// �Ƴ��Ѿ����õ�Ŀ����ϰ�������
    /// </summary>
    /// <param name="type">Ҫ���������,ȫ�����</param>
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
    /// ˢ�½�ɫ�ڵ�ͼ�ϵ�λ��
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
            Debug.LogError("���ܴ�ģ��");
        }
    }

    /// <summary>
    /// �Ƴ���ͼ�Ͻ�ɫ�ı��
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
    /// ����������ת��Ϊ��ͼ�ϵı�������(��this.graphAnchor�ı�������)
    /// </summary>
    /// <param name="pos">��������</param>
    /// <returns>
    /// flag:�Ƿ��ڵ�ͼ��Χ��
    /// pos:�ǵĻ����ڵ�ͼ�ϵı�������
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
    /// ��������ת��ͼ�ĸ�������(������ȡ������������)
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
    /// ��������ת��ͼ���ӵ��±�
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
    /// ��������Ƿ��ڵ�ͼ��
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
        Debug.LogError(str);
    }
}
