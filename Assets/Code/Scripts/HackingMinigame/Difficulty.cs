using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficultySetting", menuName = "HackingMinigame/Difficulty")]
public class Difficulty : ScriptableObject
{
    [Tooltip("How much progress is gained from each key press (note: aprox 5 keys are spawned every second)")]
    public Vector2Int myProgressRate = new Vector2Int(4, 9);
    [Tooltip("How long the player has to complete the hack")]
    public float myTimeLimit = 10f;
    [Tooltip("How much time between individual pop up spawns")]
    public Vector2 myTimeBetweenPopUps = new Vector2(2f, 4f);
    [Tooltip("How much progress the player loses per second based on the number of active pop ups")]
    public int myPopUpDecay = 4;
}
