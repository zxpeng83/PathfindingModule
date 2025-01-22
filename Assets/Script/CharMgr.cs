using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMgr : MonoBehaviour
{
    public static CharMgr instance;

    public float speed = 3.0F;
    public float rotateSpeed = 3.0F;

    public string curPos = "";

    private CharacterController myController;
    private Animator myAnimator;

    private void Awake()
    {
        instance = this;
        myAnimator = GetComponent<Animator>();
        myController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
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
