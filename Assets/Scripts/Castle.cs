﻿using System;
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

    }

    public void OnDamage()
    {

    }
}