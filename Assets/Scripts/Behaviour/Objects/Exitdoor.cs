using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Behaviour.Player;
using UnityEngine;

public class Exitdoor : MonoBehaviour
{
    Animator exitDoorAnimation;
    public Material whiteSkyBox;
    private float timer;
    private bool activated;

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
                if (activated == false)
                {
                    RenderSettings.skybox = whiteSkyBox;
                    timer += Time.deltaTime;
                    print(timer);
                    if (timer > 0.1f)
                    {
                        print("Test");
                        RenderSettings.skybox = null;
                        activated = true;
                    }

                }
                if (exitDoorAnimation.enabled == false)
                {
                    SoundManager.Instance.PlayCrack();
                    exitDoorAnimation.enabled = true;
                }
            }
        }
    }
}


