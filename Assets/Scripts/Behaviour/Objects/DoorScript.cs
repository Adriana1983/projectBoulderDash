using System.Collections;
using System.Collections.Generic;
using Behaviour.Player;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;

public class DoorScript : StateMachineBehaviour
{
    public GameObject Player;
    //Play crack sound at explosion state of the animation
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            SoundManager.Instance.PlayCrack();
    }

    //Spawn Rockford and delete door animation when animation complete
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            GameObject.Instantiate(Player, animator.gameObject.transform.position, Quaternion.identity);
            Destroy(animator.gameObject);
    }
}
