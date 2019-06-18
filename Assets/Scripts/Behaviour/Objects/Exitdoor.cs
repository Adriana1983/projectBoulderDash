using System.Collections;
using System.Collections.Generic;
using Behaviour.Player;
using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    public int score = 0;
    public Animator exitDoorAnimation;

    void Start()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == isActiveAndEnabled)
        {
            if (score >= 10)
            {
                //Sound here
                gameObject.GetComponent<Animator>().enabled = true;
            }
        }
    }
}


