using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Movement.Player
{
    public struct Direction
    {
        public Vector3 V3Direction;
        public string MoveDirection;
        public string Input;
        public string AltInput;
        
        public Direction(Vector3 v3Direction, string moveDirection, string input, string altInput)
        {
            V3Direction = v3Direction;
            MoveDirection = moveDirection;
            Input = input;
            AltInput = altInput;
        }
        
    }
    public class Movement : MonoBehaviour
    {
        public float speed = 10.0f;
        public string hitDirection;
        public string inputGot;
        public bool isHit;
        public List<Direction> directions;
        public string moveDirection;
        public Vector3 lastPos;
        public Vector3 targetPos;
        public bool mustMove;
        public bool isMoving = false;
        public Rigidbody2D player;

        private void Start()
        {
            targetPos = transform.position;
            moveDirection = "idle";
            isMoving = false;
            mustMove = false;
            //directions = new List<Direction>();

//            SetDirection( Vector3.up, "up", "W", "UpArrow");
//            SetDirection( Vector3.left,"left", "A", "LeftArrow"); 
//            SetDirection( Vector3.down,"down", "S", "DownArrow");
//            SetDirection( Vector3.right,"right", "D", "RightArrow");

        }

        private void FixedUpdate()
        {
            lastPos = transform.position;
            
            inputGot = Input.inputString;
            if (inputGot.Length < 1)
            {
                inputGot = inputGot.ToUpper(); 
            } else {
                inputGot = char.ToUpper(inputGot[0]) + inputGot.Substring(1);
            }
            
            isHit = false;
            
            // don't know if we need to store the hits each frame,
            // right now they get reset
            Dictionary<string, RaycastHit2D> raycastHit = new Dictionary<string, RaycastHit2D>();
            
            raycastHit.Add("up", Physics2D.Raycast(targetPos, Vector2.up, 1));
            raycastHit.Add("down", Physics2D.Raycast(targetPos, Vector2.down, 1));
            raycastHit.Add("right", Physics2D.Raycast(targetPos, Vector2.right, 1));
            raycastHit.Add("left", Physics2D.Raycast(targetPos, Vector2.left, 1)); 
          
            if (Input.GetKey(KeyCode.A) && !isMoving && raycastHit["left"].collider == null)
            {
                mustMove = true;
                moveDirection = "left";
                targetPos += Vector3.left;
            }

            if (Input.GetKey(KeyCode.D) && !isMoving && raycastHit["right"].collider == null)
            {
                mustMove = true;
                moveDirection = "right";
                targetPos += Vector3.right;
            }

            if (Input.GetKey(KeyCode.W) && !isMoving && raycastHit["up"].collider == null)
            {
                mustMove = true;
                moveDirection = "up";
                targetPos += Vector3.up;
            }

            if (Input.GetKey(KeyCode.S) && !isMoving && raycastHit["down"].collider == null)
            {
                mustMove = true;
                moveDirection = "down";
                targetPos += Vector3.down;
            }
            
            if (mustMove) {
                Debug.Log("Move fired");
                isMoving = true;
                Debug.Log("moviiing");
                //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                
                if (transform.position == targetPos) {
                    isMoving = false;
                    mustMove = false;
                }
            } else
            {
                moveDirection = "idle";
                Idle();
            }
            
            if (!isMoving) {
                
            }
                
            
            foreach (var hit in raycastHit)
            {
                if (hit.Value.collider != null)
                {
                    isHit = true;
                    hitDirection = hit.Key;
                }
            }
        }
        
        public void Idle()
        {
            moveDirection = "idle";
        }
        
    }
}