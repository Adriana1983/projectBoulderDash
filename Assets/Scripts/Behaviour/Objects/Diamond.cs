using System.Collections;
using System.Collections.Generic;
using Behaviour.Objects;
using Random = System.Random;

using UnityEngine;

public class Diamond : MonoBehaviour
{
    private float timer = 0.1875f;
    Random random = new Random();
    public bool falling;
    public bool lastpositionFalling;
    public bool activatedWall;

    public GameObject explosion;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1);

            if (hitDown.collider == null)
            {
                falling = true;
                transform.position += Vector3.down;
            }
            else
            {
                if (falling)
                    //SoundManager.Instance.PlayDiamondSequence();

                switch (hitDown.collider.tag)
                {
                    case "MagicWall":
                        if (lastpositionFalling || falling)
                        {
                            if (activatedWall == false)
                            {
                                hitDown.collider.gameObject.GetComponent<MagicWall>().firstactivated = true;
                                activatedWall = true;
                            }
                        }
                        break;

                    case "Wall":
                    case "Diamond":
                    case "Boulder":
                        if (hitDown.collider.CompareTag("Boulder") &&
                            hitDown.collider.gameObject.GetComponent<Boulder>().falling) break;
                        if (hitDown.collider.CompareTag("Diamond") &&
                            hitDown.collider.gameObject.GetComponent<Diamond>().falling) break;

                        RaycastHit2D hitColliderLeft =
                            Physics2D.Raycast(hitDown.point + new Vector2(0, -0.5f), Vector2.left, 1);
                        RaycastHit2D hitColliderRight =
                            Physics2D.Raycast(hitDown.point + new Vector2(0, -0.5f), Vector2.right, 1);

                        RaycastHit2D Left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                        RaycastHit2D Right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                        if (hitColliderLeft.collider == null && hitColliderRight.collider == null &&
                            Left.collider == null && Right.collider == null)
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
                        if (falling)
                        {
                            //Player death
                            DrawExplosion(hitDown);
                            Debug.Log("Player dead");
                            Destroy(hitDown.collider.gameObject);
                        }
                        break;
                    case "Firefly":
                    case "Butterfly":
                        if (falling)
                        {
                            //firefly/butterfly dies
                            DrawExplosion(hitDown);
                            Debug.Log("Firefly/Butterfly dead");
                            Destroy(hitDown.collider.gameObject);
                        }
                        break;
                }

                lastpositionFalling = falling;
                falling = false;
            }

            timer = 0.1875f;
        }
    }

    public void DrawExplosion(RaycastHit2D hit)
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


