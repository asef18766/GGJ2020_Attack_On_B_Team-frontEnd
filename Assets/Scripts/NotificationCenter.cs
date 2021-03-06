﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class NotificationCenter : MonoBehaviour
{
    #region singlton
    public static NotificationCenter ins
    {
        get
        {
            return _ins;
        }
    }
    private static NotificationCenter _ins = null;
    #endregion

    [Tooltip("Server IP address")]
    public string ip;
    [Tooltip("Server port")]
    public int port;
    [Tooltip("Buffer size for storing data coming from server (bytes)")]
    public int bufferSize;

    private Socket _clientSocket;
    private byte[] _recieveBuffer;
    private byte[] _lengthBuffer = new byte[4];
    private Dictionary<String, List<Handler>> handlers = new Dictionary<string, List<Handler>>();

    public void SetupClient()
    {
        if(ins != null)
        {
            Debug.LogError($"Multiple NotificationCenter were instantiated!");
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

        this._clientSocket.BeginReceive(this._lengthBuffer, 0, this._lengthBuffer.Length, SocketFlags.None, new AsyncCallback(HandleEvent), null);
    }



    private void HandleEvent(IAsyncResult AR)
    {
        // Check how much bytes are recieved and call EndRecieve to finalize handshake
        int recieved = this._clientSocket.EndReceive(AR);

        if(recieved <= 0)
        {
            this._clientSocket.BeginReceive(this._lengthBuffer, 0, this._lengthBuffer.Length, SocketFlags.None, new AsyncCallback(HandleEvent), null);
            return;
        }

        // Process data

        // get event data
        int len = BitConverter.ToInt32(this._lengthBuffer, 0);
        this._clientSocket.Receive(this._recieveBuffer, 0, len, SocketFlags.None);
        string recStr = System.Text.Encoding.UTF8.GetString(this._recieveBuffer);
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
                if(targetId == null || l[i].uuid == null)
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
        this._clientSocket.BeginReceive(this._lengthBuffer, 0, this._lengthBuffer.Length, SocketFlags.None, new AsyncCallback(HandleEvent), null);
    }

    public void SendData(object obj)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        byte[] lenBytes = BitConverter.GetBytes(data.Length);
        // if(BitConverter.IsLittleEndian)
        //     Array.Reverse(lenBytes);
        data = lenBytes.Concat(data).ToArray();

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