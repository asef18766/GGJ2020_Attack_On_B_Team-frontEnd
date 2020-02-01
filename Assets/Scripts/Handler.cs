using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Handler
{
    public string uuid;
    public Action<JObject> handle;

    public Handler(string uuid, Action<JObject> handle)
    {
        this.uuid = uuid;
        this.handle = handle;
    }
}
