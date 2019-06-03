using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
                RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, 1);
                if (hitleft.collider == null)
                {
                    mustMove = true;
                    moveDirection = "left";
                    targetPos += Vector3.left;
                }
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
            }

            else if (Input.GetKey(KeyCode.D) && !isMoving)
            {
                RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, 1);
                if (hitright.collider == null)
                {
                    mustMove = true;
                    moveDirection = "right";
                    targetPos += Vector3.right;
                }
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
            }

            else if (Input.GetKey(KeyCode.W) && !isMoving)
            {
                RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, 1);
                if (hitup.collider == null)
                {
                    mustMove = true;
                    moveDirection = "up";
                    targetPos += Vector3.up;
                }
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
            }

            else if (Input.GetKey(KeyCode.S) && !isMoving)
            {
                RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1);
                if (hitdown.collider == null)
                {
                    mustMove = true;
                    moveDirection = "down";
                    targetPos += Vector3.down;
                }
                else
                {
                    isHit = true;
                    hitDirection = moveDirection;
                }
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
            }
            else
            {
                Idle();
            }
        }

        public void Idle()
        {
            moveDirection = "idle";
        }
    }
}