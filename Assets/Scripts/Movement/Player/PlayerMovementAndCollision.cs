using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAndCollision : MonoBehaviour
{
    Vector3 pos;
    public float speed = 10.0f;
    public Vector2 goalvecter;

    void Start()
    {
        pos = transform.position; // Take the current position
    }

    void FixedUpdate()
    {    
        //====RayCasts====//
        RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, 1);
        RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1);
        RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, 1);
        RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, 1);
        
        //==Inputs==//
        if (Input.GetKey(KeyCode.A) && transform.position == pos && hitleft.collider == null)
        {
            pos += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D) && transform.position == pos && hitright.collider == null)
        {
            pos += Vector3.right;
        }

        if (Input.GetKey(KeyCode.W) && transform.position == pos && hitup.collider == null)
        {
            pos += Vector3.up;
        }

        if (Input.GetKey(KeyCode.S) && transform.position == pos && hitdown.collider == null)
        {
            pos += Vector3.down;
        }
       
        //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime); // Move there
      
    }
}
