using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GameMetadataParser : MonoBehaviour
{
    #region singlton
    public static GameMetadataParser ins
    {
        get
        {
            return _ins;
        }
    }
    private static GameMetadataParser _ins = null;
    #endregion

    public string filename;
    public List<GameMetadata> datas;

    public void Setup()
    {
        this.datas = new List<GameMetadata>();
        // read file
        string content = File.ReadAllText($"Assets/{this.filename}");
        // iterate array of metadata
        foreach(JObject item in JArray.Parse(content).ToArray())
        {
            GameMetadata newData = new GameMetadata();
            newData.id = Convert.ToInt32(item["ID"].Value<string>());
            newData.name = item["Name"].Value<string>();
            newData.type = Convert.ToInt32(item["Type"].Value<string>());
            newData.values = new List<string>();
            for(int i = 1; i <= 5; i++)
            {
                newData.values.Add(item[$"Value{i}"].Value<string>());
            }
            this.datas.Add(newData);
        }
    }
}

public class GameMetadata
{
    public int id;
    public string name;
    public int type;
    public List<String> values;
}