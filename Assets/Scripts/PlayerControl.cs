using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class PlayerControl : CharacterControl
{
    public PlayerKeymap keymap;

    private void move()
    {
        Vector3 del = Vector3.zero;

        if(Input.GetKey(this.keymap.up))
        {
            del += Vector3.up * (this.character.speed * Time.deltaTime);
        }
        else if(Input.GetKey(this.keymap.down))
        {
            del += Vector3.down * (this.character.speed * Time.deltaTime);
        }
        else if(Input.GetKey(this.keymap.left))
        {
            del += Vector3.left * (this.character.speed * Time.deltaTime);
        }
        else if(Input.GetKey(this.keymap.right))
        {
            del += Vector3.right * (this.character.speed * Time.deltaTime);
        }

        // move player
        this.character.transform.position += del;
        // notify server
        JObject sent = new JObject();
        sent.Add("event", "move");
        sent.Add("uuid", this.character.uuid);
        sent.Add("location", JObject.Parse(JsonUtility.ToJson(this.character.transform.position)));
        sent.Add("rotation", this.character.transform.rotation.z);
        NotificationCenter.ins.SendData(sent);
    }

    void Start()
    {
        this.character = GetComponent<Character>();        
    }

    void Update()
    {
        move();
    }
}
