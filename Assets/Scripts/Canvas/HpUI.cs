using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpUI : MonoBehaviour
{
    public Transform targetPlayer;
    Character character;

    [SerializeField]
    private Image hp;
    // Start is called before the first frame update
    void Start()
    {
        character = targetPlayer.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(targetPlayer.position.x, targetPlayer.position.y+1.5f, targetPlayer.position.z);
        hp.fillAmount = character.hp /  character.maxHP;
    }
}
