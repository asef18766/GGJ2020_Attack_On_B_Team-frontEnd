using UnityEngine;
using System.Collections;
using System.Collections.Generic;
class Respawner:MonoBehaviour
{
    public static Respawner ins
    {
        get
        {
            return _ins;
        }
    }
    private static Respawner _ins = null;
    [SerializeField] string[] teams;
    [SerializeField] Transform[] respawn_loc;
    List<Character> dead_player = new List<Character>();
    float respawn_cd = 0;
    int find_team_id(string name)
    {
        for(int u=0;u!=teams.Length;++u)
            if(teams[u] == name)
                return u;
        print("can not found team id");
        return -1;
    }
    IEnumerator player_respawn()
    {
        if(dead_player.Count != 0)
        {
            print("try to respwan player");
            Character respawn_player = dead_player[0];        
            dead_player.RemoveAt(0);

            yield return new WaitForSeconds(respawn_cd);
            respawn_player.hp=respawn_player.maxHP;
            string team_name = respawn_player.team;
            respawn_player.gameObject.SetActive(true);
            Vector3 loc = respawn_loc[ find_team_id(team_name) ].position;
            respawn_player.gameObject.transform.position = loc;
            print("respawn done!!");
        }
    }
    public void insert_dead(Character character)
    {
        dead_player.Add(character);
    }
    void Start()
    {
        if(_ins != null)
        {
            Debug.LogError($"Multiple {this.GetType()} were instantiated");
            return;
        }
        print("sucessfully create instance");
        _ins = this;
        respawn_cd = 5; // TODO: update method of initialization
    }
    void Update()
    {
        StartCoroutine(player_respawn());
    }
}