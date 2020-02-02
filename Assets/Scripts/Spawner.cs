using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Spawner : MonoBehaviour
{
    public GameObject castle;

    public void Setup()
    {
        NotificationCenter.ins.RegisterHandler("spawn", this.onSpawn, null);
    }

    private void onSpawn(JObject jo)
    {
        string uuid = jo["uuid"].Value<string>();
        Debug.Assert(uuid != null);
        string _type = jo["type"].Value<string>();
        Vector3 loc = new Vector3(
            jo["x"].Value<float>(),
            jo["y"].Value<float>(),
            jo["z"].Value<float>()
        );
        string team = jo["team"].Value<string>();
        this.Spawn(_type, uuid, team, loc);
    }

    public void Spawn(string _type, string uuid, string team, Vector3 loc)
    {
        Dictionary<string, GameObject> entities = new Dictionary<string, GameObject>();
        entities.Add("building", this.castle);

        if(!entities.ContainsKey(_type))
        {
            Debug.LogError($"Unknown entity type [{_type}]");
            return;
        }

        GameObject go = Instantiate(entities[_type], loc, Quaternion.identity);
        Entity et = go.GetComponent<Entity>();
        et.uuid = uuid;
        et.team = team;
    }
}
