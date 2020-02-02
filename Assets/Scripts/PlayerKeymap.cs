using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New PlayerKeymap")]
public class PlayerKeymap : ScriptableObject
{
    // dir
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    // action
    public KeyCode place;
    public KeyCode shop;
    public KeyCode attack;
}
