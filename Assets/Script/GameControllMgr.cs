using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

public class GameControllMgr : MonoBehaviour
{
    public static GameControllMgr instance;

    private GameMode mode;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mode = GameMode.AStar;
    }

    public void changeMode(GameMode newMode)
    {
        if (mode == newMode) return;

        mode = newMode;
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
                    CharMgr.charList[0].freshPath();
                }
                else if (rightMouseDown) //放置目标
                {
                    GraphMgr.Instance.removeTarOrBarObj(GraphObjType.Target);
                    GraphMgr.Instance.putTarOrBarObj(anchorLocalCenterPos.pos, GraphObjType.Target);
                    CharMgr.charList[0].freshPath();
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
