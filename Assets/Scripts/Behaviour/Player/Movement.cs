using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Behaviour.Player
{
    public class Movement : MonoBehaviour
    {
        public float speed = 10.0f;
        public string hitDirection;
        public string inputGot;
        public bool isHit;

        public Vector3 lastPos;
        public Vector3 targetPos;
        public bool mustMove;
        public bool isMoving;

        public int animationDirection = 0;
        public bool isIdle;
        public Animator animator;
        public float time;

        public LayerMask layer;
        public BoxCollider2D ghost;

        public int score;

        //Animation direction clockwise
        enum Direction
        {
            Idle = 0,
            Up = 1,
            Right = 2,
            Down = 3,
            Left = 4
        }

        private void Start()
        {
            targetPos = transform.position;
            isMoving = false;
            mustMove = false;
            hitDirection = "";

            //Added ghost collider to prefent boulders from falling while Rockford is not yet in target position (this couldn't be fixed with a LateUpdate!)
            //Remove Rockford as parent from ghost collider to have it work as a gameobject but still have it appear in the direction of Rockford's movement
            ghost.gameObject.transform.parent = null;
            ghost.enabled = false;
        }

        private void Update()
        {
            lastPos = transform.position;

            inputGot = Input.inputString;
            if (inputGot.Length < 1)
            {
                inputGot = inputGot.ToUpper();
            }
            else
            {
                inputGot = char.ToUpper(inputGot[0]) + inputGot.Substring(1);
            }

            isHit = false;

            //Player isn't moving, allow movement
            if (!isMoving)
            {
                //Variable for requested player direction
                Vector3 targetDirection = Vector3.zero;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    mustMove = true;
                    targetDirection = Vector3.left;
                    animationDirection = (int)Direction.Left;
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    mustMove = true;
                    targetDirection = Vector3.right;
                    animationDirection = (int)Direction.Right;
                }
                else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    mustMove = true;
                    targetDirection = Vector3.up;
                    animationDirection = (int)Direction.Up;
                }
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    mustMove = true;
                    targetDirection = Vector3.down;
                    animationDirection = (int)Direction.Down;
                }

                //Check for collision in requested player direction
                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 1, layer);

                //Did we hit anything?
                if (hit.collider != null)
                {
                    //What did we hit?
                    switch (hit.collider.gameObject.tag)
                    {
                        //We hit a boulder
                        case "Boulder":
                            //can't move boulders in stay in place mode
                            if (!Input.GetKey(KeyCode.LeftControl))
                                //call boulder hit function, decides if the player can move the boulder
                                mustMove = hit.collider.gameObject.GetComponent<Boulder>().BoulderHit(targetDirection);
                            else
                                mustMove = false;
                            break;

                        //we hit dirt
                        case "Dirt":
                            {
                                //Allow player to move
                                mustMove = true;
                                //Delete the dirt

                                SoundManager.Instance.PlayWalkDirt();
                                //Get reference to tilemap
                                var map = hit.collider.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                                //Delete tile
                                if (map != null)
                                    map.SetTile(map.WorldToCell(targetPos + targetDirection), null);
                            }
                            break;

                        case "Diamond":
                            {
                                //Allow player to move
                                mustMove = true;
                                //Delete the diamond

                                SoundManager.Instance.PlayCollectdiamond();
                                Destroy(hit.collider.gameObject);
                                score += 10;
                            }
                            break;

                        //We hit something else, player cannot move
                        default:
                            mustMove = false;
                            break;
                    }
                }

                //Did the player have to move?
                if (mustMove)
                {
                    //Check for stay in place mode, else allow player movement
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        if (hit.collider == null)
                            SoundManager.Instance.PlayWalkEmpty();

                        targetPos += targetDirection;
                        ghost.transform.position = targetPos;
                        ghost.enabled = true;

                        animator.SetInteger("AnimationDirection", animationDirection);

                        animator.SetBool("isMoving", true);
                        isMoving = true;
                    }
                }
                else
                {
                    animator.SetInteger("AnimationDirection", 0);
                    Idle();
                }
            }
            //Player is still moving
            else
            {
                //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
                if (isMoving)
                {
                    time = 0;
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                }

                //Wait for movement to finish
                if (transform.position == targetPos)
                {
                    ghost.enabled = false;
                    isMoving = false;
                    mustMove = false;
                }
            }
        }

        public void Idle()
        {
            time += Time.deltaTime;

            if (time > 10.0f)
            {
                animator.SetInteger("idle", 2);
            }
            else if (time > 5.0f)
            {
                animator.SetInteger("idle", 1);
            }
            else
            {
                animator.SetInteger("idle", 0);
            }
            isIdle = true;
            animator.SetBool("isMoving", isMoving);

        }
    }
}