using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GraphMgr.Instance = new GraphMgr();
        GraphMgr.Instance.scanGraph();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
