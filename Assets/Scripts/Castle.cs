using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Castle : Entity
{
    public int metaId;

    public int hp { get; private set; }
    public int goal { get; private set; }
    public string _filePath = "";

    private Sprite[] sprites = new Sprite[5];
    private SpriteRenderer spriteRenderer;

    // use this to setup sprites
    public override void AfterSpawn()
    {
        sprites = Resources.LoadAll<Sprite>(_filePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameMetadata data = GameMetadataParser.ins.datas[this.metaId];
        Debug.Assert(data.type == 1);
        this.hp =  Convert.ToInt32(data.values[0]);
        this.goal =  Convert.ToInt32(data.values[3]);

        NotificationCenter.ins.RegisterHandler("fix", OnFixEvent, this.uuid);
        NotificationCenter.ins.RegisterHandler("damage", OnDamageEvent, this.uuid);
        NotificationCenter.ins.RegisterHandler("end_game", OnEndGameEvent);

        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // for test
    private IEnumerator addHp()
    {
        while(true)
        {
            this.hp += 50;
            yield return new WaitForSeconds(1);
        }
    }

    private void Update()
    {
        // determine sprite by hp
        this.spriteRenderer.sprite = this.sprites[0];
        int spId = (int)(((float)this.hp / this.goal) * (this.sprites.Length - 1));
        spId = Mathf.Min(this.sprites.Length - 1, spId);
        print(spId);
        if(spId > transform.childCount)
        {
            print($"generate {spId - transform.childCount} children");
            for(int i=transform.childCount ; i<spId ; i++)
            {
                GameObject go = new GameObject(i.ToString());
                go.AddComponent<SpriteRenderer>().sprite = this.sprites[i];
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
            }
        }
        else if(spId < transform.childCount)
        {
            for(int i=transform.childCount-1 ; i>spId ; i--)
                Destroy(transform.GetChild(i));
        }
        
        // win
        if(this.hp >= this.goal)
        {
            OnEndGame(this.team);
        }
    }

    public void OnFixEvent(JObject jo) 
    {
        int? progress = jo["progress"]?.Value<int>();
        Debug.Assert(progress != null);
        OnFix((int)progress);
    }

    public void OnFix(int progress)
    {
        print(this.hp);
        this.hp = progress;
    }

    public void OnDamageEvent(JObject jo)
    {
        int healthLeft = jo["healthLeft"].Value<int>();
        int amount = jo["amount"].Value<int>();

        OnDamage(healthLeft, amount);
    }

    public void OnDamage(int healthLeft, int amount)
    {
        this.hp = healthLeft;
    }

    public void OnEndGameEvent(JObject jo)
    {
        string winner = jo["winner"]?.Value<string>();
        Debug.Assert(winner != null);
        OnEndGame(winner);
    }

    public void OnEndGame(string winTeam)
    {
        // win
        if(this.team.Equals(winTeam))
        {

        }
        // lose
        else
        {

        }
    }
}
