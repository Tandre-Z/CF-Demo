using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
//事件监听的委托类型
public delegate void MsgListener(string value);

public class NetManager
{
    public Dictionary<string,MsgListener> listeners =new Dictionary<string,MsgListener>();

    private static NetManager manager;

    public static NetManager Manager
    {
        get 
        { 
            if (manager == null)  
                manager = new NetManager();
            return manager; 
        }
    }


    private string ip="127.0.0.1";
    private int port = 8888;


    Socket socketClient;

    byte[] bytes=new byte[1024];

    Queue<string> msgQueue=new Queue<string>();

    #region 事件监听
    public void AddListener(string msgName,MsgListener listener)
    {
        if (!listeners.ContainsKey(msgName))
        {
            listeners.Add(msgName, listener);
        }
    }

    private void Invoke(string msgName, string value)
    {
        if (listeners.ContainsKey(msgName))
        {
            listeners[msgName].Invoke(value);
        }
    }
    #endregion
    public void ConnectionAsync(string ip,int port)
    {
        socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socketClient.BeginConnect(ip, port, ConnectCallback, socketClient);
    }

    /// <summary>
    /// 连接回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {

            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);

            //ipAndPort = FormatAddress(socket);

            Debug.Log("异步连接成功");
            //isConnected = true;

            bytes = new byte[1024];
            //等待接收信息
            socket.BeginReceive(bytes, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException se)
        {
            Debug.Log(se.Message);
        }
    }

    /// <summary>
    /// 接收回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socketClient = (Socket)ar.AsyncState;
            int count = socketClient.EndReceive(ar);

            if (count <= 0)
            {
                socketClient.Close();
                Debug.Log("服务器关闭连接");
                return;
            }

            string str = Encoding.UTF8.GetString(bytes, 0, count);
            Debug.LogError(socketClient + "收到的异步信息是：" + str);
            msgQueue.Enqueue(str);

            //继续等待接受信息
            socketClient.BeginReceive(bytes, 0, 1024, 0, ReceiveCallBack, socketClient);
        }
        catch (SocketException se)
        {
            Debug.Log(se.Message);
        }

    }
    /// <summary>
    /// 异步发送信息
    /// </summary>
    /// <param name="value"></param>
    public void SendAsync(string value)
    {
        byte[] sendBytes = Encoding.UTF8.GetBytes(value);
        socketClient.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socketClient);
    }
    /// <summary>
    /// 发送回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            Debug.Log("信息发送成功，长度为：" + count);
        }
        catch (SocketException se)
        {

            Debug.LogError(se.Message);
        }

    }

    
    public void CloseSocket()
    {
        if (socketClient!=null)
        {
            socketClient.Close();
        }
    }

   public void Update()
    {
        if(msgQueue.Count <= 0)
        {
            return;
        }
        string value = msgQueue.Dequeue();
        string[] cmds= value.Split('|');
        Invoke(cmds[1], value);
    }
}
