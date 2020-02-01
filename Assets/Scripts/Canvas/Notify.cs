using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class Notify : MonoBehaviour
{
    #region singlton
    public Notify ins
    {
        get
        {
            return _ins;
        }
    }
    private Notify _ins = null;
    #endregion

    [Tooltip("Server IP address")]
    public string ip;
    [Tooltip("Server port")]
    public int port;
    [Tooltip("Buffer size for storing data coming from server (bytes)")]
    public int bufferSize;

    private Socket _clientSocket;
    private byte[] _recieveBuffer;
    private Dictionary<String, List<Handler>> handlers = new Dictionary<string, List<Handler>>();

    public void SetupClient()
    {
        if(ins != null)
        {
            Debug.LogError($"Multiple Notify were instantiated!");
            return;
        }
        _ins = this;

        this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        this._recieveBuffer = new byte[this.bufferSize];

        try
        {
            this._clientSocket.Connect(new IPEndPoint(IPAddress.Parse(this.ip), this.port));
        }
        catch (SocketException ex)
        {
            Debug.Log(ex.Message);
        }

        this._clientSocket.BeginReceive(this._recieveBuffer, 0, this._recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);

    }

    private void ReceiveCallback(IAsyncResult AR)
    {
        // Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = this._clientSocket.EndReceive(AR);

        if(recieved <= 0)
            return;

        // Copy the recieved data into new buffer , to avoid null bytes
        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(this._recieveBuffer, 0, recData, 0, recieved);

        // Process data

        // get event data
        string recStr = System.Text.Encoding.UTF8.GetString(recData);
        JObject jo = JObject.Parse(recStr);
        string eventType = jo["event"].Value<String>();

        // handle
        if(!this.handlers.ContainsKey(eventType))
        {
            Debug.Log($"Unregistered event [{eventType}]");
        }
        else
        {
            // get list of handler
            List<Handler> l = this.handlers[eventType];
            string targetId = jo["uuid"]?.Value<String>();
            // trigger them
            for(int i = 0; i < l.Count; i++)
            {
                // no specific target
                if(targetId == null)
                    l[i].handle(jo);
                // target matched
                else if (targetId.Equals(l[i].uuid))
                {
                    l[i].handle(jo);
                    break;
                }
            }
        }

        // Start receiving again
        this._clientSocket.BeginReceive(this._recieveBuffer, 0, this._recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    public void SendData(object obj)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        socketAsyncData.SetBuffer(data, 0, data.Length);
        this._clientSocket.SendAsync(socketAsyncData);
    }

    public void RegisterHandler(string eventType, Action<JObject> handle, string uuid = null)
    {
        if(!this.handlers.ContainsKey(eventType))
        {
            this.handlers.Add(eventType, new List<Handler>());
        }
        this.handlers[eventType].Add(new Handler(uuid, handle));
    }
}