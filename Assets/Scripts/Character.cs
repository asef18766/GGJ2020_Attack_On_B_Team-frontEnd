using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public string characterName;
    public int maxHP;
    public int resourceCapacity;
    public float speed;
    private int hp;
    private int resource;

    public void Attack(int weaponId) {}
    public void Build(string entityType, Vector3 loc) {}
    public void Collect() {}
    public void Damage() {}
    public void purchase(string itemName) {}
}
