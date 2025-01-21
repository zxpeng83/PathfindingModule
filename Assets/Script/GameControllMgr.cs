using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    AStar,
}

public class GameControllMgr : MonoBehaviour
{
    public static GameControllMgr instance;

    public GameMode mode;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.mode = GameMode.AStar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
