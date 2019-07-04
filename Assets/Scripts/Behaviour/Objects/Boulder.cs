using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Behaviour.Creatures;
using UnityEngine;
using Behaviour.Objects;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviour.Objects
{
    public class Boulder : MonoBehaviour
    {
        private float timer = 0.1875f; //time betwee actions
        Random random = new Random();
        public bool falling;
        bool moving = false;

        private List<Vector3> explosionRadius;
        //Magic wall variables
        public bool activatedWall;
        public bool lastpositionFalling;

        public GameObject explosion;
        private Firefly firefly;
        private Butterfly butterfly;
        public GameObject diamond;

        public void Start()
        {
            firefly = GameObject.FindWithTag("Firefly").GetComponent<Firefly>();
            butterfly = GameObject.FindWithTag("Butterfly").GetComponent<Butterfly>();
           
        }

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
                    falling = true; //this boolean is used to remember if the boulder is falling
                }
                else
                {
                    Tilemap map = hit.collider.gameObject.GetComponent<Tilemap>();
                    //there is something beneath this boulder
                    switch (hit.collider.tag)
                    {
                        case "MagicWall":
                            if (lastpositionFalling || falling)
                            {
                                if (activatedWall == false)
                                {
                                    hit.collider.gameObject.GetComponent<MagicWall>().activated = true;
                                    activatedWall = true;
                                }
                            }
                            break;
                        case "Dirt":
                            if (falling)
                                SoundManager.Instance.PlayBoulder();
                            break;

                        case "Wall":
                        case "Diamond":
                        case "Boulder":

                            if (falling)
                                SoundManager.Instance.PlayBoulder();

                            //don't test collision on falling things
                            if (hit.collider.tag == "Boulder" && hit.collider.gameObject.GetComponent<Boulder>().falling) break;
                            if (hit.collider.tag == "Diamond" && hit.collider.gameObject.GetComponent<Diamond>().falling) break;

                            //check space left and right of hit object
                            RaycastHit2D hit_left = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.left, 1);
                            RaycastHit2D hit_right = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.right, 1);

                            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                            if (hit_left.collider == null && hit_right.collider == null && left.collider == null && right.collider == null)
                            {
                                //random left or right
                                if (random.Next(0, 1) == 0)
                                {
                                    transform.position = transform.position + Vector3.left;
                                    falling = true;
                                }
                                else
                                {
                                    transform.position = transform.position + Vector3.right;
                                    falling = true;
                                }
                            }
                            //left space is empty Boulder falls left
                            else if (hit_left.collider == null && left.collider == null)
                            {
                                transform.position = transform.position + Vector3.left;
                                falling = true;
                            }
                            //right space is empty Boulder falls right
                            else if (hit_right.collider == null && right.collider == null)
                            {
                                transform.position = transform.position + Vector3.right;
                                falling = true;
                            }
                            break;

                        case "Player":
                            if (falling)
                            {
                                //player dies
                                DrawExplosion(hit.transform.transform.position);
                                Debug.Log("Player dead");
                                Destroy(hit.collider.gameObject);

                            }
                            break;

                        case "Firefly":
                            if (falling)
                            {
                                DrawExplosion(map.WorldToCell(gameObject.transform.position));
                            }
                            break;
                        case "Butterfly":
                            if (falling)
                            {
                                DrawExplosion(map.WorldToCell(gameObject.transform.position));
                                DrawDiamonds(map.WorldToCell(gameObject.transform.position), map);
                                
                            }
                            break;

                        default:
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

        public void DrawDiamonds(Vector3 pos, Tilemap map)
        {
            CreateList(pos);
            foreach (var block in explosionRadius)
            {
                if (!map.HasTile(new Vector3Int((int) block.x, (int) block.y, 0)))
                {
                    InstantiatePrefab(diamond, block);
                    //diamond.layer = LayerMask.NameToLayer("Diamonds");
                }
            }
        }
    }
}
