using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchBtn : MonoBehaviour
{
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
        Notify.SendData(playerName);
    }
}
