using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_EscapeDoor : MonoBehaviour
{
    //when teh player reaches the end of the level, they have to go back to the start and exit. This is becus the game is designed with backtracking in mind 


    //in the game manager, include an object called the "escape door" which disappears when the player has collected all the cards. When the player enters this collider, *level complete*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.LevelComplete();
    }
}
