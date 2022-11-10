using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_SingletonPattern : MonoBehaviour
{
    //Note this code is not my own, it is taken from Blackboard

    private static Proto_SingletonPattern _instance = null;

    public static Proto_SingletonPattern instance
    {
        get
        {
            //check if this this script exists in the scene after being made through the awake method
            if (_instance == null)
            {
                //check the scene to see if this script already exists somewhere
                _instance = FindObjectOfType<Proto_SingletonPattern>();
                if (_instance == null)
                {
                    //if it doesnt, create it
                    GameObject me = new GameObject();
                    me.name = "Prototype Singleton Design Pattern";
                    _instance = me.AddComponent<Proto_SingletonPattern>();
                    DontDestroyOnLoad(me);
                }
            }

            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null)
        {
            //if instance already exists, then this isn't the original, and so it should destroy itself
            Destroy(this);
        }
        else
        {
            //if this is the original copy of this script, then it should be the instance
            _instance = this;
            //Dont destroy on load stops an object from being destroyed when a new scene is opened. 
            DontDestroyOnLoad(this.gameObject);
        }
    }

    int score = 0;
    public void ChangeScore(int scoreChange)
    {
        score += scoreChange;
        Debug.Log(score);
    }
}
