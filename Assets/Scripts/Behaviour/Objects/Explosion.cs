using System.Collections;
using System.Collections.Generic;
using Behaviour.Creatures;
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
                //Get tile layer
                var map = hit.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                switch (hit.tag)
                {
                    //All deletable tiles
                    case "Dirt":
                    case "Wall":
                        
                        //Find target tile on found layer and delete it
                        map.SetTile(map.WorldToCell(animator.gameObject.transform.position), null);
                        break;
                    // tiles that need to be deleted inside a list too
                    case "Butterfly":
                        GameObject.FindWithTag("Butterfly").GetComponent<Butterfly>()
                            .DestroyButterfly(map.WorldToCell(animator.gameObject.transform.position));
                        //Get tile layer
                        map = hit.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                        //Find target tile on found layer and delete it
                        map.SetTile(map.WorldToCell(animator.gameObject.transform.position), null);
                        break;
                    case "Firefly":
                        GameObject.FindWithTag("Firefly").GetComponent<Firefly>()
                            .DestroyFirefly(map.WorldToCell(animator.gameObject.transform.position));
                        //Get tile layer
                        map = hit.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                        //Find target tile on found layer and delete it
                        map.SetTile(map.WorldToCell(animator.gameObject.transform.position), null);
                        break;
                    case "Amoeba":
//                        GameObject.FindWithTag("Amoeba").GetComponent<Amoeba>()
//                            .DestroyAmoeba(map.WorldToCell(animator.gameObject.transform.position));
//                        //Get tile layer
                        map = hit.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                        //Find target tile on found layer and delete it
                        map.SetTile(map.WorldToCell(animator.gameObject.transform.position), null);
                        break;
                    //All deletable GameObjects
                    case "Player":
                    case "Boulder":
                    case "Diamond": 
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
