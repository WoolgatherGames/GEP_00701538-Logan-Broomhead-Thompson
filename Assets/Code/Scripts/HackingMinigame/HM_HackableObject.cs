using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HM_HackableObject : MonoBehaviour
{
    [SerializeField] Difficulty hackingDifficulty;
    bool HackAlreadyComplete;

    private void Start()
    {
        HackAlreadyComplete = false;
    }

    public bool HackInteract()
    {
        //called by objects that inherit from Iinteractable when they are clicked on
        //return if they have been hacked or not, and if they havn't. Start the hacking minigame
        if (HackAlreadyComplete)
        {
            //if the object has been hacked, enable its functionality
            return true;
        }
        else
        {
            //hack the object
            HM_HackingManager.instance.BeginHack(hackingDifficulty, this);
        }

        return false;
    }

    public void HackCompleted()
    {
        //called by the hacking manager when this objects hack was completed. 
        //Call the interact function on the object this is attatched too once the hack is done. 
        this.gameObject.GetComponent<IInteractables>().Interact();
    }
}
