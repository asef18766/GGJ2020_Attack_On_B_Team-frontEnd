using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Entity
{
    [SerializeField] int level = 0;
    static List<float> amount = null;
    [SerializeField] float unit ; 
    
    float cur_hold = 0;

    // Start is called before the first frame update
    void Start()
    {
        if( amount == null )
        {
            amount = new List<float>(new float[4]{0,2,4,5});
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
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Character>()!=null)
        {
            Character character = collision.gameObject.GetComponent<Character>();
            character.Collect();
        }
    }
}
