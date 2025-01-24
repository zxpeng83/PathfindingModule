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

        if (isHitSomething) //������
        {
            ///�Ƿ���Ѿ����õ��ϰ���
            bool isHitBarrier = hitInfo.collider.gameObject.name.IndexOf("Barrier") != -1;
            ///�Ƿ���Ѿ����õ�Ŀ��
            bool isHitTarget = hitInfo.collider.gameObject.name.IndexOf("Target") != -1;

            var anchorLocalCenterPos = GraphMgr.Instance.worldPos2AnchorLocalCenterPos(hitInfo.point);
            var graphIdx = GraphMgr.Instance.worldPos2GraphIdx(hitInfo.point);
            var charGraphIdx = CharMgr.charList[0].getGraphIdx();
            ///�Ƿ�򵽽�ɫ
            bool isHitChar = (graphIdx.flag && graphIdx.pos.Equals(charGraphIdx.pos));

            if (isHitBarrier || isHitChar || !anchorLocalCenterPos.flag) //��֮ǰ���õ��ϰ��� �� ���ڽ�ɫ���ڸ����� �� ���ڵ�ͼ֮��
            {
                GraphMgr.Instance.clearAllFakeObj();
                return;
            }
            else  //���λ�úϷ�,�ɸ�������
            {
                if (leftMouseDown) //�����ϰ���
                {
                    GraphMgr.Instance.putTarOrBarObj(anchorLocalCenterPos.pos, GraphObjType.Barrier);
                    CharMgr.charList[0].freshPath();
                }
                else if (rightMouseDown) //����Ŀ��
                {
                    GraphMgr.Instance.removeTarOrBarObj(GraphObjType.Target);
                    GraphMgr.Instance.putTarOrBarObj(anchorLocalCenterPos.pos, GraphObjType.Target);
                    CharMgr.charList[0].freshPath();
                }
                else //����Ԥ���������
                {
                    GraphMgr.Instance.putFakeObj(anchorLocalCenterPos.pos, GraphObjType.Fake);
                }
            }
        }
        else //û������
        {
            GraphMgr.Instance.clearAllFakeObj();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
