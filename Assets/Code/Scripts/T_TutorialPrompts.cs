using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_TutorialPrompts : MonoBehaviour
{
    //attatch this to the tutorial messages on the screen. 
    //cause tutorial messages to disapear when the player passes them 
    //to do: add in one that says WASD to move, and maybe an explanation of how the hacking system works. (specifically that pop ups reduce your progress cus that doesnt seem as intuitive as the flashing keyboard keys)


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            this.gameObject.SetActive(false);
    }
}
