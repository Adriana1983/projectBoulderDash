using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Boulder : MonoBehaviour
{
    public SoundManager soundManager;
    private float timer = 0.15f; //time betwee actions
    Random random = new Random();
    public bool Falling;
    bool moving = false;

    public GameObject explosion;

    //Magic wall variables
    public bool activatedWall;
    public bool LastpositionFalling;

    //Return bool decides if rockford is allowed to move in the movement script
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

        //Rockford can't move boulders up or down
        if (targetDirection == Vector3.up || targetDirection == Vector3.down)
            return false;

        //Check if there is something behind the boulder in the direction we want to push
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 1);
        if (hit.collider != null)
            //There is something in the way, rockford cannot push this boulder
            return false;

        //There was nothing, so move the boulder to the empty square
        SoundManager.Instance.PlayBox_push();
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
                    case "Dirt":
                        if (Falling)
                            SoundManager.Instance.PlayBoulder();
                        break;

                    case "Wall":
                    case "Diamond":
                    case "Boulder":

                        if (Falling)
                            SoundManager.Instance.PlayBoulder();

                        //don't test collision on falling things
                        if (hit.collider.tag == "Boulder" && hit.collider.gameObject.GetComponent<Boulder>().Falling) break;
                        if (hit.collider.tag == "Diamond" && hit.collider.gameObject.GetComponent<Diamond>().Falling) break;

                        //check space left and right of hit object
                        RaycastHit2D hit_left = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.left, 1);
                        RaycastHit2D hit_right = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.right, 1);

                        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                        if (hit_left.collider == null && hit_right.collider == null && left.collider == null & right.collider == null)
                        {
                            //random left or right
                            if (random.Next(0, 1) == 0)
                            {
                                transform.position = transform.position + Vector3.left;
                            }
                            else
                            {
                                transform.position = transform.position + Vector3.right;
                            }
                        }
                        //left space is empty Boulder falls left
                        else if (hit_left.collider == null && left.collider == null)
                        {
                            transform.position = transform.position + Vector3.left;
                        }
                        //right space is empty Boulder falls right
                        else if (hit_right.collider == null && right.collider == null)
                        {
                            transform.position = transform.position + Vector3.right;
                        }
                        break;

                    case "Player":
                        if (Falling)
                        {
                            //player dies
                            DrawExplosion(hit);
                            Debug.Log("Player dead");
                        }
                        break;

                    case "Firefly":
                    case "Butterfly":
                        if (Falling)
                        {
                            //firefly/butterfly dies
                            DrawExplosion(hit);
                            Debug.Log("Firefly/Butterfly dead");
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

    private void DrawExplosion(RaycastHit2D hit)
    {
        //Draw 3x3 explosion grid
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up + Vector3.left, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up + Vector3.right, Quaternion.identity);

        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.left, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.right, Quaternion.identity);

        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down + Vector3.left, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down, Quaternion.identity);
        GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down + Vector3.right, Quaternion.identity);

        SoundManager.Instance.PlayExplosion();
    }
}
