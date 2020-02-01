using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchBtn : MonoBehaviour
{
    public Notify notify;

    private string playerName;
    // Start is called before the first frame update
    void Start()
    {
        playerName = GetComponentInChildren<InputField>().text; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnClick(){
        playerName = GetComponentInChildren<InputField>().text; 
        notify.SendData(playerName);
    }
}
