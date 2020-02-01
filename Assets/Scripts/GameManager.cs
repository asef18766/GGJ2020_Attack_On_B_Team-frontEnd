using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private NotificationCenter notificationCenter;
    private GameMetadataParser gameMetadataParser;
    private Spawner spawner;

    void Start()
    {
        // get components
        this.notificationCenter = GetComponent<NotificationCenter>();    
        this.gameMetadataParser = GetComponent<GameMetadataParser>();
        this.spawner = GetComponent<Spawner>();

        // setup
        this.notificationCenter.SetupClient();
        this.gameMetadataParser.Setup();
        this.spawner.Setup();

        DontDestroyOnLoad(gameObject);
    }
}
