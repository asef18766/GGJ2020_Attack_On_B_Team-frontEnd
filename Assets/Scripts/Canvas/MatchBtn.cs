using System.Collections;
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

    public void btnClick()
    {
        playerName = input.text;
        if(playerName.Equals(""))
            return;
        particleSys.SetActive(true);
        ani.SetTrigger("Match");

        JObject sent = new JObject();
        sent.Add("event", "connect");
        sent.Add("playerName", playerName);
        notificationCenter.SendData(sent);
    }
}