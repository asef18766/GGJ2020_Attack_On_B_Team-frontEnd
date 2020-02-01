using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    private NotificationCenter notificationCenter;

    void Start()
    {
        // get components
        this.notificationCenter = GetComponent<NotificationCenter>();    

        // setup
        this.notificationCenter.SetupClient();

        DontDestroyOnLoad(gameObject);
    }
}
