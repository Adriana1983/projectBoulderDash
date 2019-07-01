using System.Collections;
using System.Collections.Generic;
using Behaviour.Player;
using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    Animator exitDoorAnimation;

    void Start()
    {
        exitDoorAnimation = gameObject.GetComponent<Animator>();
        exitDoorAnimation.enabled = false;
    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == isActiveAndEnabled)
        {
            if (Score.Instance.diamondsCollected >= Score.Instance.diamondsNeeded)
            {
                if (exitDoorAnimation.enabled == false)
                    SoundManager.Instance.PlayCrack();

                exitDoorAnimation.enabled = true;
            }
        }
    }
}


