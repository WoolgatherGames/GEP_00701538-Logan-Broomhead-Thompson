using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_PopUpScript : MonoBehaviour
{
    private void OnMouseDown()
    {
        Prototype_HackingManager.instance.PopUpDestroyed();
        Destroy(this.gameObject);
    }
}
