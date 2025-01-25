using GameConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldNode : IFsmNode
{
    private string _name = nameof(FlowFieldNode);
    public string Name => this._name;

    public void OnEnter()
    {
        GraphMgr.Instance.removeTarOrBarObj(GraphObjType.Target);
        GraphMgr.Instance.removeTarOrBarObj(GraphObjType.Barrier);
        //CharMgr.char
        //throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }
}
