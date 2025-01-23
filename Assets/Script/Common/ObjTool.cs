using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ObjTool
{
    private static ObjTool ins = null;

    public static ObjTool instance
    {
        get
        {
            if(ins == null)
            {
                ins = new ObjTool();
            }

            return ins;
        }
    }

    /// <summary>
    /// 设置物体的名字, 包括所有子物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    public void setNameWithChild(GameObject go,  string name)
    {
        go.name = name;

        Transform[] children = go.GetComponentsInChildren<Transform>();

        foreach (var item in children)
        {
            item.gameObject.name = name;
        }
    }

    /// <summary>
    /// 设置物体的层级,包括所有子物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="lay"></param>
    public void setLayWithChild(GameObject go, int lay)
    {
        go.layer = lay;

        Transform[] children = go.GetComponentsInChildren<Transform>();

        foreach (var item in children)
        {
            item.gameObject.layer = lay;
        }
    }
}
