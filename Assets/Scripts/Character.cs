using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Character : Entity
{
    public string characterName;
    public int maxHP;
    public int resourceCapacity;
    public float speed;
    private int hp;
    public int resource;

    public void Attack(int weaponId) {}
    public void Build(string entityType, Vector3 loc) {}
    public void Collect() {}
    public void Damage() {}
    public void purchase(string itemName) {}

    public bool UpgradeGenerator()
    {
        return Input.GetKey(KeyCode.U);
    }
    public void OnDamageEvent(JObject jo)
    {
        this.hp = jo["healthLeft"].Value<int>();
    }

    public void OnKillEvent(JObject jo)
    {
        // play dead ani

        // respawn player
    }

    private void Start()
    {
        NotificationCenter.ins.RegisterHandler("damage", OnDamageEvent, this.uuid);
        NotificationCenter.ins.RegisterHandler("kill", OnKillEvent, this.uuid);
    }
}
