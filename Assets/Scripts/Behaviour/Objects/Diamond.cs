using System.Collections;
using System.Collections.Generic;
using Behaviour.Objects;
using Random = System.Random;

using UnityEngine;
using UnityEngine.Tilemaps;

public class Diamond : MonoBehaviour
{
    private float timer = 0.1875f;
    Random random = new Random();
    public bool falling;
    public bool lastpositionFalling;
    public bool activatedWall;

    public GameObject explosion;
    private List<Vector3> explosionRadius;

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
                    SoundManager.Instance.PlayDiamondSequence();

                switch (hitDown.collider.tag)
                {
                    case "MagicWall":
                        if (lastpositionFalling)
                        {
                            if (activatedWall == false)
                            {
                                hitDown.collider.gameObject.GetComponent<MagicWall>().activated = true;
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
                            DrawExplosion(hitDown.transform.position);
                            Debug.Log("Player dead");
                            Destroy(hitDown.collider.gameObject);
                            Score.Instance.RockfordDies();
                        }
                        break;
                    case "Firefly":
                    case "Butterfly":
                        if (falling)
                        {
                            Tilemap map = hitDown.collider.gameObject.GetComponent<Tilemap>();
                            DrawExplosion(map.WorldToCell(gameObject.transform.position));
                        }
                        break;
                }

                lastpositionFalling = falling;
                falling = false;
            }

            timer = 0.1875f;
        }
    }

    public void DrawExplosion(Vector3 position)
    {
        CreateList(position);
        foreach (var block in explosionRadius)
        {
            InstantiatePrefab(explosion, block);
        }

        SoundManager.Instance.PlayExplosion();
    }

    public void InstantiatePrefab(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity);
    }

    public void CreateList(Vector3 position)
    {
        explosionRadius = new List<Vector3>
        {
            position + Vector3.up + Vector3.left,
            position + Vector3.up,
            position + Vector3.up + Vector3.right,
            position + Vector3.left,
            position,
            position + Vector3.right,
            position + Vector3.down + Vector3.left,
            position + Vector3.down,
            position + Vector3.down + Vector3.right
                
        };
    }

}

