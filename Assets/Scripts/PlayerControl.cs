using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class PlayerControl : CharacterControl
{
    public PlayerKeymap keymap;
    
    private void moveAndRotate()
    {
        // should update
        bool u = false;

        // face
        Vector3 faceTo = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        faceTo.z = 0;
        faceTo = -faceTo.normalized;
        if(faceTo != this.character.transform.up)
        {
            this.character.transform.up = faceTo;
            u = true;
        }

        // position delta
        Vector3 posDelta = Vector3.zero;
        // vertical
        if(Input.GetKey(this.keymap.up))
        {
            posDelta += Vector3.up * (this.character.speed * Time.deltaTime);
        }
        else if(Input.GetKey(this.keymap.down))
        {
            posDelta += Vector3.down * (this.character.speed * Time.deltaTime);
        }
        // horizontal
        if(Input.GetKey(this.keymap.left))
        {
            posDelta += Vector3.left * (this.character.speed * Time.deltaTime);
        }
        else if(Input.GetKey(this.keymap.right))
        {
            posDelta += Vector3.right * (this.character.speed * Time.deltaTime);
        }
        // move player
        if(posDelta != Vector3.zero)
        {
            this.character.transform.position += posDelta;
            u = true;
        }

        // notify server
        if(u)
        {
            JObject sent = new JObject();
            sent.Add("event", "move");
            sent.Add("uuid", this.character.uuid);
            sent.Add("location", JObject.Parse(JsonUtility.ToJson(this.character.transform.position)));
            sent.Add("rotation", this.character.transform.rotation.z);
            NotificationCenter.ins.SendData(sent);
        }
    }

    void Start()
    {
        this.character = GetComponent<Character>();
    }

    void Update()
    {
        moveAndRotate();
    }
}
