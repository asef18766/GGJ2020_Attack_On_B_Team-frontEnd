using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerControl : CharacterControl
{
    public PlayerKeymap keymap;

    private void moveAndRotate()
    {
        // should update
        bool u = false;

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

        if(posDelta != Vector3.zero)
        {
            // move player
            this.character.GetComponent<Rigidbody2D>().AddForce(posDelta);
            // face
            this.character.transform.up = -posDelta.normalized;
            u = true;
        }

        // notify server
        // we never need them again, QAQ
        // if(u)
        // {
        //     JObject sent = new JObject();
        //     sent.Add("event", "move");
        //     sent.Add("uuid", this.character.uuid);
        //     sent.Add("location", JObject.Parse(JsonUtility.ToJson(this.character.transform.position)));
        //     sent.Add("rotation", this.character.transform.rotation.z);
        //     NotificationCenter.ins.SendData(sent);
        // }
    }

    private void attack()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2) this.character.transform.position, this.character.attackRange, Vector2.zero);
        Character[] cs = hits.Select(h => h.collider.GetComponent<Character>())
            .Where(c => c != null)
            .ToArray();
        foreach (Character c in cs)
        {
            c.Damage();
        }
    }

    void Start()
    {
        this.character = GetComponent<Character>();
    }

    void Update()
    {
        this.moveAndRotate();
        this.attack();
    }
}