using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class MouseMgr : MonoBehaviour
{
    public static MouseMgr instance;

    public GameObject graphAnchor;
    private GameObject obj = null;
    public (int minX, int maxX) rangeX = (minX: 1, maxX: 21);
    public (int minZ, int maxZ) rangeZ = (minZ: 1, maxZ: 21);

    private void Awake()
    {
        MouseMgr.instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GraphMgr.Instance.prin();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool isHitSomthing = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1<<2));
        bool leftMouseDown = Input.GetMouseButtonDown(0);
        bool rightMouseDown = Input.GetMouseButtonDown(1);


        if (isHitSomthing)
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point);
        }

        if (isHitSomthing) //打到物体
        {
            bool isHitBarrier = hitInfo.collider.gameObject.name.IndexOf("Barrier") != -1;
            var graphPos = this.getPosInGraph(hitInfo.point);

            //Debug.LogFormat("isHitBarrier:{0},  posInGraph.flag:{1},  posInGraph.pos:{2}", isHitBarrier, graphPos.flag, graphPos.pos);

            if(isHitBarrier || !graphPos.flag) //打到之前放置的障碍物 或 打在地图之外
            {
                this.backObj();
                return;
            }
            else  //打的位置合法,可跟随或放置
            {
                if (Input.GetMouseButtonDown(0)) //放置
                {
                    this.putObj(graphPos.pos);
                }
                else //跟随
                {
                    this.moveObj(graphPos.pos);
                }
            }
        }
        else //没打到物体
        {
            this.backObj();
        }
    }
    private (bool flag, Vector3 pos) getPosInGraph(Vector3 pos)
    {
        //Debug.LogFormat("before: {0}", pos);
        Vector3 localPos = graphAnchor.transform.InverseTransformPoint(pos);
        //Debug.Log(localPos);
        if(localPos.x <= this.rangeX.minX || localPos.x >= this.rangeX.maxX
            || localPos.z <= this.rangeZ.minZ || localPos.z >= this.rangeZ.maxZ
            || localPos.y <= -0.15 || localPos.y >= 0.15)
        {
            return (flag: false, pos: Vector3.zero);
        }

        Vector3 rtnPos = new Vector3(Mathf.Floor(localPos.x) + 0.5f, 0, Mathf.Floor(localPos.z) + 0.5f);

        return (flag: true, pos: rtnPos);
    }

    private GameObject getObj()
    {
        if (obj == null) obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = "Barrier";
        obj.transform.parent = this.graphAnchor.transform;
        obj.layer = 2;
        return obj;
    }

    private void backObj()
    {
        if(obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }

    private void destroyObj()
    {
        if(obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }

    private void putObj(Vector3 pos)
    {
        this.getObj();

        obj.transform.localPosition = pos;
        obj.layer = 0;
        obj.name = "BarrierActive_" + Mathf.Floor(pos.x) + "_" + Mathf.Floor(pos.z);
        GraphMgr.Instance.setVal((int)Mathf.Floor(pos.x), (int)Mathf.Floor(pos.z), 1);
        obj = null;
    }

    private void moveObj(Vector3 pos)
    {
        this.getObj();

        obj.transform.localPosition = pos;
    }
}
