using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

class AStarData
{
    public Vector3 targetPos;
    public int targetCount = 0;
    public GameObject targetGo;

    public AStarData()
    {
        this.targetPos = Vector3.zero;
        this.targetCount = 0;
        this.targetGo = null;
    }

    public bool addTarget(GameObject go, Vector3 pos)
    {
        if(targetCount == 0)
        {
            targetCount++;
            targetPos = pos;
            targetGo = go;
            return true;
        }
        return false;
    }

    public void removeTarget()
    {
        if(targetCount > 0)
        {
            targetCount--;
            targetPos = Vector3.zero;
            ObjPool.instance.backObj(targetGo);
        }
    }
}

public class GameControllMgr : MonoBehaviour
{
    public static GameControllMgr instance;

    public GameMode mode;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mode = GameMode.AStar;
    }

    public void mouseInput(RaycastHit hitInfo, bool isHitSomething, bool leftMouseDown, bool rightMouseDown)
    {
        switch (this.mode)
        {
            case GameMode.AStar:
                this.mouseInputAStar(hitInfo,  isHitSomething, leftMouseDown, rightMouseDown);
                break;
            default:
                break;
        }
    }

    private void mouseInputAStar(RaycastHit hitInfo, bool isHitSomething, bool leftMouseDown, bool rightMouseDown)
    {
        if (isHitSomething)
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point);
        }

        if (isHitSomething) //打到物体
        {
            ///是否打到已经放置的障碍物
            bool isHitBarrier = hitInfo.collider.gameObject.name.IndexOf("Barrier") != -1;
            ///是否打到已经放置的目标
            bool isHitTarget = hitInfo.collider.gameObject.name.IndexOf("Target") != -1;

            var anchorLocalCenterPos = GraphMgr.Instance.worldPos2AnchorLocalCenterPos(hitInfo.point);
            var graphIdx = GraphMgr.Instance.worldPos2GraphIdx(hitInfo.point);
            var charGraphIdx = CharMgr.charList[0].getGraphIdx();
            ///是否打到角色
            bool isHitChar = (graphIdx.flag && graphIdx.pos.Equals(charGraphIdx.pos));

            if (isHitBarrier || isHitChar || !anchorLocalCenterPos.flag) //打到之前放置的障碍物 或 打在角色所在格子里 或 打在地图之外
            {
                GraphMgr.Instance.clearAllFakeObj();
                return;
            }
            else  //打的位置合法,可跟随或放置
            {
                if (leftMouseDown) //放置障碍物
                {
                    GraphMgr.Instance.putTarOrBarObj(anchorLocalCenterPos.pos, GraphObjType.Barrier);
                }
                else if (rightMouseDown) //放置目标
                {
                    GraphMgr.Instance.removeTarOrBarObj(GraphObjType.Target);
                    GraphMgr.Instance.putTarOrBarObj(anchorLocalCenterPos.pos, GraphObjType.Target);
                }
                else //放置预瞄物体跟随
                {
                    GraphMgr.Instance.putFakeObj(anchorLocalCenterPos.pos, GraphObjType.Fake);
                }
            }
        }
        else //没打到物体
        {
            GraphMgr.Instance.clearAllFakeObj();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
