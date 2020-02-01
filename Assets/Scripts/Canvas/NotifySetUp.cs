using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifySetUp : MonoBehaviour
{
    private Notify notify;
    void Start()
    {
        // setup
        this.notify = GetComponent<Notify>();

        notify.SetupClient();
    }

}
