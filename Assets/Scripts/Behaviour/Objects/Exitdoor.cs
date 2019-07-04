using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Behaviour.Player;
using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    Animator exitDoorAnimation;
    public Material Whiteskybox;
    private float timer;

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
                RenderSettings.skybox = Whiteskybox;
                timer += Time.deltaTime;
                if (timer >= 0.15f)
                {
                    RenderSettings.skybox = null;
                    timer = 0;
                }   
                exitDoorAnimation.enabled = true;
            }
        }
    }
}


