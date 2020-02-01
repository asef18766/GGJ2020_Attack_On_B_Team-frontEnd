using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Castle : Entity
{
    public int metaId;

    public int hp { get; private set; }
    public int goal { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameMetadata data = GameMetadataParser.ins.datas[this.metaId];
        Debug.Assert(data.type == 1);
        this.hp =  Convert.ToInt32(data.values[0]);
        this.goal =  Convert.ToInt32(data.values[3]);

        NotificationCenter.ins.RegisterHandler("fix", OnFixEvent, this.uuid);
        NotificationCenter.ins.RegisterHandler("damage", OnDamageEvent, this.uuid);
        NotificationCenter.ins.RegisterHandler("end_game", OnEndGameEvent);
    }

    private void Update()
    {
        // determine sprite by hp
    }

    public void OnFixEvent(JObject jo) 
    {
        int? progress = jo["progress"]?.Value<int>();
        Debug.Assert(progress != null);
        OnFix((int)progress);
    }

    public void OnFix(int progress)
    {
        this.hp = progress;
    }

    public void OnDamageEvent(JObject jo)
    {
        int healthLeft = jo["healthLeft"].Value<int>();
        int amount = jo["amount"].Value<int>();

        OnDamage(healthLeft, amount);
    }

    public void OnDamage(int healthLeft, int amount)
    {
        this.hp = healthLeft;
    }

    public void OnEndGameEvent(JObject jo)
    {
        string winner = jo["winner"]?.Value<string>();
        Debug.Assert(winner != null);
        OnEndGame(winner);
    }

    public void OnEndGame(string winTeam)
    {
        // win
        if(this.team.Equals(winTeam))
        {

        }
        // lose
        else
        {

        }
    }
}
