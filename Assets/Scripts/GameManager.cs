using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NotificationCenter notificationCenter;
    private GameMetadataParser gameMetadataParser;

    void Start()
    {
        // get components
        this.notificationCenter = GetComponent<NotificationCenter>();    
        this.gameMetadataParser = GetComponent<GameMetadataParser>();

        // setup
        this.notificationCenter.SetupClient();
        this.gameMetadataParser.Setup();

        DontDestroyOnLoad(gameObject);
    }
}
