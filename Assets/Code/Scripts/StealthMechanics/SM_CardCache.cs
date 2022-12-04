using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_CardCache : MonoBehaviour
{
    //put this script on the win conditions the player has to collect. make sure they have a 2d trigger collider. 
    //The win condition. The player picks these up. once all are collected, the player completes the level. or maybe they need to make an escape
    //send a msg to the game manager that this has been collected. 

    //the lore of the game is that the player is breaking into the card factory for the popular trading card game "Hide and Mossy" Theyre trying to get their hands on the super rare cards so they can win the next tournament against their rival
    //^ I did want to have an opening cutscene type thingy. but doesnt look like ill have time for it. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.AddCard();
            Destroy(this.gameObject);
        }
    }
}
