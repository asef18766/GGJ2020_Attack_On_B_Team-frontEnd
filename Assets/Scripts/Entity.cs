﻿using UnityEngine;

public class Entity : MonoBehaviour
{
    public string uuid;
    public string team;

    public virtual void AfterSpawn() {}
}
