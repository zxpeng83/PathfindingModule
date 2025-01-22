using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ∂‘œÛ≥ÿ
/// </summary>
public class ObjPool
{
    private static ObjPool ins;
    public static ObjPool instance
    {
        get
        {
            if(ins == null)
            {
                ins = new ObjPool();
            }

            return ins;
        }
    }

    private Dictionary<string, List<GameObject>> pool;
    private Dictionary<string, GameObject> prefabs;

    public ObjPool()
    {
        this.pool = new Dictionary<string, List<GameObject>>();
        this.prefabs = new Dictionary<string, GameObject>();
    }

    public GameObject getObj(string prefabName)
    {
        GameObject rtnObj = null;
        if (prefabs.ContainsKey(prefabName) && pool.ContainsKey(prefabName) && pool[prefabName].Count>0)
        {
            rtnObj = pool[prefabName][0];
            pool[prefabName].RemoveAt(0);
        }
        else
        {
            if(!prefabs.ContainsKey(prefabName))
            {
                GameObject loadPrefab = Resources.Load<GameObject>("Prefab/" +  prefabName);
                prefabs.Add(prefabName, loadPrefab);
                pool.Add(prefabName, new List<GameObject>());
            }
            if (prefabs[prefabName] == null) return null;

            rtnObj = Object.Instantiate(prefabs[prefabName]);
        }

        rtnObj.name = prefabName;

        rtnObj.SetActive(true);
        return rtnObj;
    }

    public void backObj(GameObject obj)
    {
        if(obj.name.IndexOf("-") != -1)
        {
            string[] tem = obj.name.Split('-');
            obj.name = tem[0];
        }
        if(obj.name.IndexOf("_") != -1)
        {
            string[] tem = obj.name.Split("_");
            obj.name = tem[0];
        }

        if (!pool.ContainsKey(obj.name))
        {
            Object.Destroy(obj);
        }
        else
        {
            obj.SetActive(false);
            pool[obj.name].Add(obj);
        }
    }
}
