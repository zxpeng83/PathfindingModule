using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast
{
    private static RayCast ins = null;
    public static RayCast instance
    {
        get
        {
            if (ins == null)
            {
                ins = new RayCast();
            }

            return ins;
        }
    }

    /// <summary>
    /// ½ÇÉ«Â·¾¶
    /// </summary>
    private List<Vector2> path = new List<Vector2>();


}
