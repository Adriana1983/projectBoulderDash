using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using Behaviour.Objects;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Behaviour.Creatures
{
    public class Amoeba : MonoBehaviour
    {
        public string hitDirection;
        public bool isHit;

        public Vector3 targetPos;
        private Vector3 hitPos;
        private Vector3 previous;
        public bool mustGrow;
        public bool isGrowing;
        public int maxSize;
        public int amoebaCount;

        public LayerMask layer;
        public Tilemap dirtTileMap;
        public float nextGrow;
        public float growSpeed;

        public List<Vector3> allowedDirections;

        private void Start()
        {
            targetPos = transform.position;
            previous = targetPos;
            isGrowing = false;
            mustGrow = false;
            hitDirection = "";
        }

        private void Update()
        {
            amoebaCount = GameObject.FindGameObjectsWithTag("Amoeba").Length;
            hitPos = targetPos;
            allowedDirections = new List<Vector3>();
            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector3.left, 1);
            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector3.right, 1);
            RaycastHit2D down = Physics2D.Raycast(transform.position, Vector3.down, 1);
            RaycastHit2D up = Physics2D.Raycast(transform.position, Vector3.up, 1);
            
            if (left.collider == null || left.collider.CompareTag("Dirt"))
            {
                allowedDirections.Add(Vector3.left);
            }
            
            if (right.collider == null || right.collider.CompareTag("Dirt"))
            {
                allowedDirections.Add(Vector3.right);
            }
            
            if (down.collider == null || down.collider.CompareTag("Dirt"))
            {
                allowedDirections.Add(Vector3.down);
            }
            
            if (up.collider == null || up.collider.CompareTag("Dirt"))
            {
                allowedDirections.Add(Vector3.up);
            }
                
           
            
            // limit grow speed by looking at time
            // this script fires every "growSpeed" seconds
            if (Time.time > nextGrow)
            {
                if (allowedDirections.Count == 0)
                {
                    mustGrow = false;
                }
                else
                {
                    // get random direction to grow for amoeba;
                    var random = new Random();
                    int index = random.Next(allowedDirections.Count);
                    Vector3 direction = allowedDirections[index];
                    hitPos += direction;
                    
                    //Check for collision in requested player direction
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPos, 1, layer);
                    Debug.DrawRay(previous, direction, Color.red);
                    //Did we hit anything?
                    if (hit.collider != null)
                    {
                        //What did we hit?
                        switch (hit.collider.gameObject.tag)
                        {
                            case "Dirt":
                                //Allow amoeba to grow
                                mustGrow = true;
                                targetPos = hitPos;
                                break;
                            case "Butterfly":
                                break;
                            case "Firefly":
                                break;
                            //We hit something else, amoeba cannot grow
                            default:
                                mustGrow = false;
                                break;
                        }
                    }
                    else
                    {
                        targetPos = hitPos;
                        mustGrow = true;
                    }
                    
                    // Can amoeba grow?
                    if (mustGrow)
                    {
                        // Dont allow amoebas to grow more than this
                        if (amoebaCount < maxSize)
                        {
                            // if the position has changed
                            if (targetPos != previous)
                            {
                                Instantiate(GameObject.Find("Amoeba"), targetPos, new Quaternion(0, 0, 0, 0));
                                //Delete the dirt
                                if (dirtTileMap != null)
                                {
                                    dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
                                }
                                isGrowing = true;
                                // save the new position for next frame
                                previous = targetPos;
                            }
                        }
                    }
                }
                nextGrow += growSpeed;
            }
           
        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
          
        }
    }
}