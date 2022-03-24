using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.Manager.AddListener("Enter", OnEnter);
        NetManager.Manager.AddListener("Move", OnMove);
        NetManager.Manager.AddListener("Leave", OnLeave);
        NetManager.Manager.ConnectionAsync("127.0.0.1",8888);
    }

    private void OnLeave(string value)
    {
        Debug.Log("OnLeave" + value);
    }

    private void OnMove(string value)
    {
        Debug.Log("OnMove" + value);
    }

    private void OnEnter(string value)
    {
        Debug.Log("OnEnter"+value);
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Manager.Update();
    }
}
