using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ServerControl : CharacterControl
{
    void Start()
    {
        string uuid = this.character.uuid;

        NotificationCenter.ins.RegisterHandler("move", OnMoveEvent, uuid);
    }

    public void OnMoveEvent(JObject jo)
    {
        // position
        Vector3 loc = new Vector3(
            jo["x"].Value<float>(),
            jo["y"].Value<float>(),
            jo["z"].Value<float>()
        );
        this.character.transform.position = loc;
        
        // rotation
        float rot = jo["rotation"].Value<float>();
        Vector3 ea = this.character.transform.eulerAngles;
        ea.z = rot;
        this.character.transform.eulerAngles = ea;
    }

    public void OnAttack(JObject jo)
    {
        int weapon = jo["weapon"].Value<int>();
    }
}
