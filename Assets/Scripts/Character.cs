using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Character : Entity
{
    public string characterName;
    public int maxHP;
    public int resourceCapacity;
    public float speed;
    public float attackRange;

    public int hp;
    public int resource;

    private IEnumerator fixCoroutine;

    public void Attack(int weaponId) { }
    public void Build(string entityType, Vector3 loc) { }
    public void Collect(int amount) { }
    public void Damage()
    {
        const int dmg = 10;
        this.hp -= dmg;
    }
    public void purchase(string itemName) { }

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
        hp = maxHP;
    }

    void UpdateHealthEvent()
    {
        if(hp <= 0)
        {
            print("dead...QAQ");
            print("instance null:" + (Respawner.ins == null).ToString());
            Respawner.ins.insert_dead(this);
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        UpdateHealthEvent();
    }

    IEnumerator fix(Castle castle)
    {
        const int amount = 10;
        while(true)
        {
            if(this.resource > amount)
            {
                castle.OnFix(castle.hp + amount);
                this.resource -= amount;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Castle castle = other.gameObject.GetComponent<Castle>();
        if(castle == null || !castle.team.Equals(this.team)) return;

        this.fixCoroutine = fix(castle);
        StartCoroutine(this.fixCoroutine);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Castle castle = other.gameObject.GetComponent<Castle>();
        if(castle == null || !castle.team.Equals(this.team)) return;

        if(this.fixCoroutine != null)
        {
            StopCoroutine(this.fixCoroutine);
            this.fixCoroutine = null;
        }
    }
}