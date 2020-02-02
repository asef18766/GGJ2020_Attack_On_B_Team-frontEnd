using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Entity
{
    [SerializeField] int level = 0;
    static List<float> amount = null;
    static List<float> update_need = null;
    const int MAX_LEVEL = 4;
    [SerializeField] float unit ; 
    float cur_hold = 0;

    // Start is called before the first frame update
    void Start()
    {
        if( amount == null )
        {
            amount = new List<float>(new float[MAX_LEVEL]{0,2,4,5});
        }
        if( update_need == null )
        {
            update_need = new List<float>(new float[MAX_LEVEL]{0,1,2,3});
        }
    }
    bool block = false;
    IEnumerator UpdateResource()
    {
        yield return new WaitForSeconds(unit);
        block = false;
        cur_hold += (amount[level] / unit) ;
    }
    // Update is called once per frame
    void Update()
    {
        if(!block)
        {
            block = true;
            StartCoroutine(UpdateResource());
        }
        // Debug.Log("cur_hold " + cur_hold.ToString());
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<Character>()!=null)
        {
            Character character = collider.gameObject.GetComponent<Character>();
            character.resource += (int)cur_hold;
            cur_hold = 0;

            bool player_update = character.UpgradeGenerator(); // TODO: update api
            // check player want to upgrade
            if(player_update)
            {
                print("player wait to upgrade");
                // check if it's top level generator
                if(level != MAX_LEVEL-1)
                {
                    // check resouce need
                    if(character.resource >= update_need[level + 1])
                    {
                        level ++;
                        character.resource -= (int)update_need[level + 1];
                    }
                }

            }
        }
    }
}
