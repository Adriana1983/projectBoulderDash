using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

using UnityEngine;

public class Diamond : MonoBehaviour
{
    private float Timer = 0.15f;
    Random random = new Random();
    public bool Falling;
    public bool LastpositionFalling;
    public bool activatedWall;

    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer < 0)
        {
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1);

            if (hitDown.collider == null)
            {
                Falling = true;
                transform.position += Vector3.down;
            }
            else
            {
                switch (hitDown.collider.tag)
                {
                    case "MagicWall":
                        if (LastpositionFalling)
                        {
                            if (activatedWall == false)
                            {
                                hitDown.collider.gameObject.GetComponent<MagicWall>().activated = true;
                                activatedWall = true;
                            }
                        }
                        break;
                    case "Diamond":
                    case "Boulder":
                            if (hitDown.collider.CompareTag("Boulder") &&
                                hitDown.collider.gameObject.GetComponent<Boulder>().Falling) break;
                            if (hitDown.collider.CompareTag("Diamond") &&
                                hitDown.collider.gameObject.GetComponent<Diamond>().Falling) break;

                            RaycastHit2D hitColliderLeft =
                                Physics2D.Raycast(hitDown.collider.transform.position, Vector2.left, 1);
                            RaycastHit2D hitColliderRight =
                                Physics2D.Raycast(hitDown.collider.transform.position, Vector2.right, 1);

                            RaycastHit2D Left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                            RaycastHit2D Right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                            if (hitColliderLeft.collider == null && hitColliderRight.collider == null &&
                                Left.collider == null && Right.collider)
                            {
                                if (random.Next(0, 1) == 0)
                                {
                                    transform.position += Vector3.right;
                                }
                                else
                                {
                                    transform.position += Vector3.left;
                                }
                            }
                            else if (Left.collider == null && hitColliderLeft.collider == null)
                            {
                                transform.position += Vector3.left;
                            }
                            else if (Right.collider == null && hitColliderRight.collider == null)
                            {
                                transform.position += Vector3.right;
                            }


                        break;
                case "Player":
                if (Falling)
                {
                    //Player death
                }

                break;
                }

                LastpositionFalling = Falling;
                Falling = false;
            }

            Timer = 0.15f;
        }
    }
}


