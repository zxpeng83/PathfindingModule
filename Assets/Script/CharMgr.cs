using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharMgr : MonoBehaviour
{
    public static List<CharMgr> charList = new List<CharMgr>();

    //public float speed = 3.0F;
    //public float rotateSpeed = 3.0F;

    //private (float x, float z) localPos;

    private CharacterController myController;
    private Animator myAnimator;
    //// 上一帧的位置数据
    //private (bool flag, Vector3 pos) preAnchorLocalPos;
    //private (bool flag, Vector3 pos) preAnchorLocalCenterPos;
    //private (bool flag, Vector3 pos) preGraphIdx;
    /// <summary>
    /// 自动寻路的路径
    /// </summary>
    private List<Vector2Int> path = new List<Vector2Int>();

    /// <summary>
    /// 是否是主控角色(只有一个主控角色能被操控)
    /// </summary>
    public bool isMaster = false;


    private void Awake()
    {
        CharMgr.charList.Add(this);
        CharMgr.charList[0].isMaster = true;

        myAnimator = GetComponent<Animator>();
        myController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //this.freshPreData();
        //this.myReset();
    }

    /// <summary>
    /// 获取角色在地图上的本地坐标(相当于graphAhchor的本地坐标)
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getAnchorLocalPos()
    {
        return GraphMgr.Instance.worldPos2AnchorLocalPos(gameObject.transform.position);
    }

    /// <summary>
    /// 获取角色在地图上的格子坐标(格子内取格子中心坐标)
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getAnchorLocalCenterPos()
    {
        return GraphMgr.Instance.worldPos2AnchorLocalCenterPos(gameObject.transform.position);
    }

    /// <summary>
    /// 获取角色在地图格子的下标
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getGraphIdx()
    {
        return GraphMgr.Instance.worldPos2GraphIdx(gameObject.transform.position);
    }

    /// <summary>
    /// 刷新寻路路径
    /// </summary>
    public void freshPath()
    {
        this.path = AStar.instance.startNavigation();
    }

    /// <summary>
    /// 重置角色，重置之前记得先清楚地图上的障碍物和目标
    /// </summary>
    /// <param name="graphIdx">地图格子的下标</param>
    public void myReset(Vector2 graphIdx = default(Vector2))
    {
        if(graphIdx != default(Vector2))
        {
            transform.localPosition = new Vector3(graphIdx.x+0.5f, 0, graphIdx.y + 0.5f);
        }

        this.gameObject.name = "Char" + "-" + this.getGraphIdx().pos.x + "-" + this.getGraphIdx().pos.z;

        GraphMgr.Instance.refreshChar();

        this.freshPath();
    }

    ///// <summary>
    ///// 重置(只有主控端才可以重置，非主控端只能销毁；且重置前得先清楚地图上的物体）
    ///// </summary>
    //public void reset()
    //{
    //    if (!this.isMaster) return;

    //    this.path.Clear();

    //    GraphMgr.Instance.removeChar(this.preGraphIdx.pos);
    //    GraphMgr.Instance.removeChar(this.getGraphIdx().pos);

    //    transform.localPosition = new Vector3(1, 0, 1);

    //    this.gameObject.name = "Char" + "-" + this.getGraphIdx().pos.x + "-" + this.getGraphIdx().pos.z;

    //    GraphMgr.Instance.freshChar(this.getGraphIdx().pos);

    //    this.freshPreData();
    //}

    // Update is called once per frame


    void Update()
    {
        GameControllMgr.instance.charUpdate(this);
    }

    /// <summary>
    /// 通过键盘输入手动操控玩家移动
    /// </summary>
    public void moveByKeyboard()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v).normalized;

        if (Mathf.Abs(dir.x) < 1e-3 && Mathf.Abs(dir.z) < 1e-3)
        {
            myAnimator.SetBool("isRun", false);
            return;
        }
        else
        {
            myAnimator.SetBool("isRun", true);
        }

        transform.rotation = Quaternion.LookRotation(dir);
        myController.SimpleMove(dir * 4);

        //GraphMgr.Instance.removeChar(this.preGraphIdx.pos);

        //if (this.getGraphIdx().flag)
        //{
        //    GraphMgr.Instance.freshChar(this.getGraphIdx().pos);
        //}

        //this.freshPreData();
        GraphMgr.Instance.refreshChar();

        this.gameObject.name = "Char" + "-" + this.getGraphIdx().pos.x + "-" + this.getGraphIdx().pos.z;

        Debug.LogFormat("{0} , {1} , {2}", h, v, dir);
    }

    /// <summary>
    /// 自动寻路
    /// </summary>
    public void moveByAuto()
    {
        while(this.path != null && this.path.Count > 0) 
        {
            Vector2 nexPointV2 = this.path.Last();
            nexPointV2.x += 0.5f;
            nexPointV2.y += 0.5f;
            var curPointV2 = new Vector2(this.getAnchorLocalPos().pos.x, this.getAnchorLocalPos().pos.z);
            if (MathTool.getEuclideanDisV2(nexPointV2, curPointV2) < 0.1)
            {
                this.path.RemoveAt(path.Count - 1);
                continue;
            }

            Vector2 dirV2 = (nexPointV2 - curPointV2).normalized;
            Vector3 dirV3 = new Vector3(dirV2.x, 0, dirV2.y);
            myAnimator.SetBool("isRun", true);
            transform.rotation = Quaternion.LookRotation(dirV3);
            myController.SimpleMove(dirV3 * 2);

            //GraphMgr.Instance.removeChar(this.preGraphIdx.pos);

            //if (this.getGraphIdx().flag)
            //{
            //    GraphMgr.Instance.freshChar(this.getGraphIdx().pos);
            //}

            //this.freshPreData();
            GraphMgr.Instance.refreshChar();

            this.gameObject.name = "Char" + "-" + this.getGraphIdx().pos.x + "-" + this.getGraphIdx().pos.z;

            break;
        }

        if(this.path == null || this.path.Count <= 0)
        {
            this.rotation2Target();
            myAnimator.SetBool("isRun", false);
        }
    }

    ///// <summary>
    ///// 刷新上一帧的数据
    ///// </summary>
    //private void freshPreData()
    //{
    //    this.preAnchorLocalPos = this.getAnchorLocalPos();
    //    this.preAnchorLocalCenterPos = this.getAnchorLocalCenterPos();
    //    this.preGraphIdx = this.getGraphIdx();
    //}

    /// <summary>
    /// 使角色转向目标
    /// </summary>
    private void rotation2Target()
    {
        Vector3? target = AStar.instance.getCurTarget<Vector3>();
        if (target == null) return;

        Vector3 dir = (Vector3)target - this.getAnchorLocalPos().pos;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
