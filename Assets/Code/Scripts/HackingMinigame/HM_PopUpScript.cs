using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM_PopUpScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        HM_HackingManager.instance.PopUpDestroyed();
        Destroy(this.gameObject);
    }
}