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
        public Rigidbody2D player;
        public float speed = 10.0f;
        public string hitDirection;
        public string inputGot;
        public bool isHit;
        public string moveDirection;
        public Vector3 lastPos;
        public Vector3 targetPos;
        public bool mustMove;
        public bool isMoving;
        public bool isRight;
        public bool isIdle;
        public Animator animator;
        public float time;
        public LayerMask layer;
        public Tilemap dirtTileMap;

        private void Start()
        {
            targetPos = transform.position;
            moveDirection = "idle";
            isMoving = false;
            mustMove = false;
            hitDirection = "";
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

            if (Input.GetKey(KeyCode.A) && !isMoving)
            {
                RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, 1, layer);
                animator.SetBool("isRight", false);
                if (hitleft.collider == null)
                {
                    mustMove = true;
                    moveDirection = "isLeft";
                    targetPos += Vector3.left;
                }
                else if (hitleft.collider.CompareTag("Dirt"))
                {
                    mustMove = true;
                    moveDirection = "isLeft";
                    targetPos += Vector3.left;
                    if (dirtTileMap != null)
                    {
                        dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
                    }
                } 
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                    
                }
                animator.SetBool(moveDirection, true);
                animator.SetBool("isMoving", true);
            }

            else if (Input.GetKey(KeyCode.D) && !isMoving)
            {
                RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, 1, layer);
                if (hitright.collider == null)
                {
                    mustMove = true;
                    moveDirection = "isRight";
                    targetPos += Vector3.right;
                }
                else if (hitright.collider.CompareTag("Dirt"))
                {
                    mustMove = true;
                    moveDirection = "isRight";
                    targetPos += Vector3.right;
                    if (dirtTileMap != null)
                    {
                        dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
                    }
                } 
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
                animator.SetBool(moveDirection, true);
                animator.SetBool("isMoving", true);
            }

            else if (Input.GetKey(KeyCode.W) && !isMoving)
            {
                RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, 1, layer);
                animator.SetBool("isRight", false);
                if (hitup.collider == null)
                {
                    mustMove = true;
                    moveDirection = "isUp";
                    targetPos += Vector3.up;
                }
                else if (hitup.collider.CompareTag("Dirt"))
                {
                    mustMove = true;
                    moveDirection = "isUp";
                    targetPos += Vector3.up;
                    if (dirtTileMap != null)
                    {
                        dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
                    }
                } 
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
                
                animator.SetBool(moveDirection, false);
                animator.SetBool("isMoving", true);
            }

            else if (Input.GetKey(KeyCode.S) && !isMoving)
            {
                RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1, layer);
                animator.SetBool("isRight", false);
                if (hitdown.collider == null)
                {
                    mustMove = true;
                    moveDirection = "isDown";
                    targetPos += Vector3.down;
                }
                else if (hitdown.collider.CompareTag("Dirt"))
                {
                    mustMove = true;
                    moveDirection = "isDown";
                    targetPos += Vector3.down;
                    if (dirtTileMap != null)
                    {
                        dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
                    }
                } 
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
                animator.SetBool(moveDirection, true);
                animator.SetBool("isMoving", true);
            }

            if (mustMove)
            {
                isMoving = true;

                //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

                if (transform.position == targetPos)
                {
                    isMoving = false;
                    mustMove = false;
                }
                time = 0;
            }
            else
            {
                Idle();
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