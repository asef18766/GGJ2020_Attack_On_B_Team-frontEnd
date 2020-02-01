﻿using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchBtn : MonoBehaviour
{
    public NotificationCenter notificationCenter;
    public GameObject particleSys;
    public InputField input;

    Animator ani;

    private string playerName;
    // Start is called before the first frame update
    void Start()
    {
        playerName = input.text;
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnClick()
    {
        particleSys.SetActive(true);
        ani.SetTrigger("Match");
        playerName = input.text;

        JObject sent = new JObject();
        sent.Add("event", "connect");
        sent.Add("playerName", playerName);
        notificationCenter.SendData(sent);
    }
}