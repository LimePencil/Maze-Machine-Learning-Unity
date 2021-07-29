using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System;

public class ToPython : MonoBehaviour
{
    Thread mThread;
    public string connectionIP;
    public int connectionPort;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos;
    private GameObject playerObj;
    float[] inputVal;
    bool running;
    byte[] data_sent;
    public bool reset;
    PlayerMovement movementScript;

    private void Update()
    {
        byte[] xVal = BitConverter.GetBytes(playerObj.transform.position.x);
        byte[] zVal = BitConverter.GetBytes(playerObj.transform.position.x);
        for(int i = 0; i < 4; i++)
        {
            data_sent[i + 1] = xVal[i];
            data_sent[i + 5] = zVal[i];
        }

    }

    private void Start()
    {
        connectionIP = "127.0.0.1";
        connectionPort = 25002;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        movementScript = playerObj.GetComponent<PlayerMovement>();
        inputVal = new float[2];
        data_sent = new byte[9];
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];


        //---receiving Data from the Host----
        nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        inputVal[0] = BitConverter.ToSingle(buffer.SubArray(0,4),0);

        inputVal[1] = BitConverter.ToSingle(buffer.SubArray(4, 4),0);
        reset = Convert.ToBoolean(buffer[8]);
        movementScript.receivedData = true;
        //---Sending Data to Host----
        data_sent[0] = (byte)(GoalScript.done ? 1 : 0);
        nwStream.Write(data_sent, 0, data_sent.Length);
    }
    public float[] getAgentInput()
    {
        return inputVal;
    }

    private void OnApplicationQuit()
    {
        mThread.Abort();
        client.Close();
    }
}
public static class Extensions
{
    public static T[] SubArray<T>(this T[] array, int offset, int length)
    {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }
}


