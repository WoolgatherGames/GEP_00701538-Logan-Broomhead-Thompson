using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM_PopUpScript : MonoBehaviour
{
    //pop ups should destroy themself when clicked
    private void OnMouseDown()
    {
        HM_HackingManager.instance.PopUpDestroyed();
        Destroy(this.gameObject);
    }

    //important life lesson: If theres 2 cameras in the scene both rendering to a display. OnMouseDown stops working. im not 100% clear on why, but keep that in mind
    //^ its totally fine with a camera sending to a render camera btw. 
}