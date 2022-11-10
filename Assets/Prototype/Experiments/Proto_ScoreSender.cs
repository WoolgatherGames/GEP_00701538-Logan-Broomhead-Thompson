using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_ScoreSender : MonoBehaviour
{
    private void Awake()
    {
        Proto_SingletonPattern.instance.ChangeScore(1);
    }

    private void Update()
    {
         if (Input.GetKeyDown(KeyCode.Space))
        {
            Proto_SingletonPattern.instance.ChangeScore(1); 
        }
    }
}
