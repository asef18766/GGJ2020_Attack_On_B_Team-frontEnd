using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text txt;
    public int estTime;
    public bool startPair;
    bool underEstTime;
    int currentTime;
    float timer_f = 0f;
    float minutes;
    float seconds;

    // Start is called before the first frame update
    void Start()
    {
        startPair = false;
        underEstTime = true;
        txt = GetComponent<Text>();
        currentTime = estTime;
    }

    // Update is called once per frame
    void Update()
    {
        //print(currentTime);
        if(startPair){
            if(timer_f < 1f){
                timer_f += Time.deltaTime;
            }
            else{
                timer_f = 0f;
                if(currentTime>14 && underEstTime){
                    currentTime -= 1;
                }
                else if(currentTime < 0){
                    underEstTime = false;
                    txt.color = Color.red;
                    currentTime += 1;
                }
            }
        }
        System.TimeSpan t = System.TimeSpan.FromSeconds(currentTime);
        string niceTime = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        txt.text = niceTime;
    }
}
