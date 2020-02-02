using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
public class WaitingRoom:MonoBehaviour
{
    [SerializeField] string des = "";
    void Start()
    {
        NotificationCenter.ins.RegisterHandler("waiting" , waiting_handler);
        NotificationCenter.ins.RegisterHandler("enter_game" , enter_game_handler);
    }
    void enter_game_handler(JObject jObject)
    {
        Debug.Log("change scence!!!");
        // change_scence();
    }
    void change_scence()
    {
        SceneManager.LoadScene(des);
    }
    void waiting_handler(JObject jObject)
    {
        Debug.Log("waiting_event: "+jObject.ToString());
    }
    public void ready()
    {
        JObject jObject = new JObject();
        jObject.Add("event" , "ready");
        NotificationCenter.ins.SendData(jObject);
    }
}