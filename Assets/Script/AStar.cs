using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static AStar ins = null;
    public static AStar instance
    {
        get
        {
            if(ins == null)
            {
                ins = new AStar();
            }

            return ins;
        }
    }

    public void startNavigation()
    {
        if (CharMgr.charList.Count <= 0) return;

        var charac = CharMgr.charList[0].getGraphIdx();
        if (!charac.flag) return;

        int[][] graph = GraphMgr.Instance.getGraph();


    }
}
