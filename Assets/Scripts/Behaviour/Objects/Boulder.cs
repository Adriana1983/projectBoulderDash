using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;
using Random = System.Random;

public class Boulder : MonoBehaviour
{
    private float timer = 0.15f; //time between actions
    Random random = new Random();
    public bool Falling;
    bool moving = false;
    public bool activatedWall;
    public bool LastpositionFalling;

    public bool BoulderHit(Vector3 targetDirection)
    {
        //if (moving || random.Next(0, 100) > 90)
        //{
        //    moving = true;
        //}
        //else
        //{
        //    Debug.Log("too bad");
        //    return false;
        //}

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 1);
        if (hit.collider != null)
            return false;

        transform.position += targetDirection;
        return true;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            //check if there is anything below this boulder (transform is this boulder)
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1);
            if (hit.collider == null)
            {
                //there's nothing beneath this boulder
                transform.position += Vector3.down;
                Falling = true; //this boolean is used to remember if the boulder is falling
            }
            else
            {
                //there is something beneath this boulder
                switch (hit.collider.tag)
                {
                    case "MagicWall":
                        if (LastpositionFalling)
                        {
                            if (activatedWall == false)
                            {
                                hit.collider.gameObject.GetComponent<MagicWall>().activated = true;
                                activatedWall = true;
                            }
                        }
                        break;
                    case "Diamond":
                    case "Boulder":
                        //Don't test collision on falling things
                        if (hit.collider.tag == "Boulder" && hit.collider.gameObject.GetComponent<Boulder>().Falling) break;
                        if (hit.collider.tag == "Diamond" && hit.collider.gameObject.GetComponent<Diamond>().Falling) break;
                        Falling = false;
                        //Check space left and right of hit object
                        RaycastHit2D hit_left = Physics2D.Raycast(hit.collider.transform.position, Vector2.left, 1);
                        RaycastHit2D hit_right = Physics2D.Raycast(hit.collider.transform.position, Vector2.right, 1);

                        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                        if (hit_left.collider == null && hit_right.collider == null && left.collider == null & right.collider == null)
                        {
                            //random left or right
                            if (random.Next(0,1) == 0)
                            {
                                transform.position = hit.collider.transform.position + Vector3.left;
                            }
                            else
                            {
                                transform.position = hit.collider.transform.position + Vector3.right;
                            }
                        }
                        //Left space is empty rock falls left
                        else if (hit_left.collider == null && left.collider == null)
                        {
                            transform.position = hit.collider.transform.position + Vector3.left;
                        }
                        //Left space is empty rock falls right
                        else if (hit_right.collider == null && right.collider == null)
                        {
                            transform.position = hit.collider.transform.position + Vector3.right;
                        }
                        break;

                    case "Player":
                        if (Falling)
                        {
                            //Player dies
                            Debug.Log("Player dead");
                        }
                        break;



                    default:
                        break;
                }
                LastpositionFalling = Falling;
                Falling = false;
            }

            timer = 0.15f;
        }
    }
}
