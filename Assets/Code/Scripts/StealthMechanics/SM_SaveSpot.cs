using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_SaveSpot : MonoBehaviour
{
    //send a location to the game manager, so the player can respawn from this location.
    //in lv1 set one in the hub bit of the level, and at the end of every wing

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.SetRespawnLocation(this.transform.position);
        }
    }
}
