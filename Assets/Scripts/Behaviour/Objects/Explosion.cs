using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : StateMachineBehaviour
{
    //At start of explosion animation delete things that are on the same tile
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //List of found colliders
        List<Collider2D> colliders = new List<Collider2D>();

        //Get explosision collider and find other colliders that overlap with it
        var collider = animator.gameObject.GetComponent<BoxCollider2D>();
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        //Disable collider so objects don't rest on top of explosions
        collider.enabled = false;


        if (colliders.Count > 0)
        {
            foreach (var hit in colliders)
            {
                switch (hit.tag)
                {
                    //All deletable tiles
                    case "Dirt":
                    case "Wall":
                        //Get tile layer
                        var map = hit.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                        //Find target tile on found layer and delete it
                        map.SetTile(map.WorldToCell(animator.gameObject.transform.position), null);
                        break;

                    //All deletable GameObjects
                    case "Player":
                    case "Boulder":
                        Destroy(hit.gameObject);
                        break;
                }
            }
        }

    }

    //Destroy explosion game object when animation done
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Destroy(animator.gameObject);
    }
}
