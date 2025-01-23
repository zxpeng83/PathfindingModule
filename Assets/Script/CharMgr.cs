using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharMgr : MonoBehaviour
{
    public static CharMgr instance;

    //public float speed = 3.0F;
    //public float rotateSpeed = 3.0F;

    //private (float x, float z) localPos;

    private CharacterController myController;
    private Animator myAnimator;

    private void Awake()
    {
        instance = this;
        myAnimator = GetComponent<Animator>();
        myController = GetComponent<CharacterController>();
        //this.localPos.x = gameObject.transform.localPosition.x;
        //this.localPos.z = gameObject.transform.localPosition.z;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    /// <summary>
    /// ��ȡ��ɫ�ڵ�ͼ�ϵı�������(�൱��graphAhchor�ı�������)
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getAnchorLocalPos()
    {
        return GraphMgr.Instance.worldPos2AnchorLocalPos(gameObject.transform.position);
    }

    /// <summary>
    /// ��ȡ��ɫ�ڵ�ͼ�ϵĸ�������(������ȡ������������)
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getAnchorLocalCenterPos()
    {
        return GraphMgr.Instance.worldPos2AnchorLocalCenterPos(gameObject.transform.position);
    }

    /// <summary>
    /// ��ȡ��ɫ�ڵ�ͼ���ӵ��±�
    /// </summary>
    /// <returns></returns>
    public (bool flag, Vector3 pos) getGraphIdx()
    {
        return GraphMgr.Instance.worldPos2GraphIdx(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
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
        Debug.LogFormat("{0} , {1} , {2}", h, v, dir);
    }
}
