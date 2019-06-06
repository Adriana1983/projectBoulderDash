﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Behaviour.Objects
{
    public class Diamond : MonoBehaviour
    {
        //Things to add: Score Counter, Player pickup diamond, diamond kills player.
        private float timer = 0.15f;
        Random random = new Random();
        private bool moved;
        int leftorright = 0;
        private int Score;
        public bool Falling;
        public LayerMask layer;

        void Update()
        {
            Vector2 positionrayup =
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f);
            Vector2 positionraydown =
                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
            Vector2 positionrayright =
                new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y);
            Vector2 positionrayleft =
                new Vector2(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y);

            Vector2 positionrayleftdown =
                new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y - 0.5f);
            Vector2 positionrayrightdown =
                new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y - 0.5f);

            Vector2 positionrayrightup =
                new Vector2(gameObject.transform.position.x + 1f, gameObject.transform.position.y + 0.5f);
            Vector2 positionrayleftup =
                new Vector2(gameObject.transform.position.x - 1f, gameObject.transform.position.y + 0.5f);

            RaycastHit2D hitup = Physics2D.Raycast(positionrayup, Vector2.up, 0.5f, layer);
            RaycastHit2D hitdown = Physics2D.Raycast(positionraydown, Vector2.down, 0.5f, layer);
            RaycastHit2D hitright = Physics2D.Raycast(positionrayright, Vector2.right, 0.5f, layer);
            RaycastHit2D hitleft = Physics2D.Raycast(positionrayleft, Vector2.left, 0.5f, layer);

            RaycastHit2D hitleftdown = Physics2D.Raycast(positionrayleftdown, Vector2.down, 0.5f, layer);
            RaycastHit2D hitrightdown = Physics2D.Raycast(positionrayrightdown, Vector2.down, 0.5f, layer);

            RaycastHit2D hitrightup = Physics2D.Raycast(positionrayrightup, Vector2.up, 1.5f, layer);
            RaycastHit2D hitleftup = Physics2D.Raycast(positionrayleftup, Vector2.up, 1.5f, layer);

            //Tester rays
//
            Debug.DrawRay(positionrayup, Vector2.up, Color.red);
            Debug.DrawRay(positionraydown, Vector2.down, Color.blue);
            Debug.DrawRay(positionrayright, Vector2.right, Color.magenta);
            Debug.DrawRay(positionrayleft, Vector2.left, Color.yellow);

//            Debug.DrawRay(positionrayleftdown, Vector2.down, Color.white);
//            Debug.DrawRay(positionrayrightdown, Vector2.down, Color.white);
//
//            Debug.DrawRay(positionrayrightup, Vector2.up, Color.white);
//            Debug.DrawRay(positionrayleftup, Vector2.up, Color.white);

            timer -= Time.deltaTime;

            if (timer < 0)
            {
                //If down raycast is empty
                if (hitdown.collider == null)
                {
                    Vector2 Newposition = new Vector2(gameObject.transform.position.x,
                        gameObject.transform.position.y - 1);
                    gameObject.transform.position = Newposition;
                    moved = true;
                    Falling = true;
                }

                //If down raycast is niet empty
                if (hitdown.collider != null)
                {
                    Debug.Log("collision" + hitdown.collider);
                    //Falling = false;
                    //If down is Diamond and up is empty, checkt of de diamond bovenop staat zodat hij kan vallen
                    if (hitdown.collider != null && hitup.collider == null)
                    {
                        if (hitdown.collider.CompareTag("Diamond") || hitdown.collider.CompareTag("Bounds") ||
                            hitdown.collider.CompareTag("Boulder") || hitdown.collider.CompareTag("Player") ||
                            hitdown.collider.CompareTag("Dirt"))
                        {
                            //If left and right raycasts are empty
                            if (hitright.collider == null && hitleft.collider == null && moved == false)
                            {
                                //If leftdown is niet empty en rechtsdown is empty
                                if (hitleftdown.collider != null && hitrightdown.collider == null &&
                                    hitrightup.collider == null)
                                {
                                    Vector2 Newposition = new Vector2(gameObject.transform.position.x + 1,
                                        gameObject.transform.position.y);
                                    gameObject.transform.position = Newposition;
                                    moved = true;
                                }

                                //If rechtsdown is niet empty en linksdown is empty
                                if (hitrightdown.collider != null && hitleftdown.collider == null &&
                                    hitleftup.collider == null)
                                {
                                    Vector2 Newposition = new Vector2(gameObject.transform.position.x - 1,
                                        gameObject.transform.position.y);
                                    gameObject.transform.position = Newposition;
                                    moved = true;
                                }

                                //If beide linksdown en rechtsdown empty zijn
                                if (hitleftdown.collider == null && hitrightdown.collider == null)
                                {
                                    //If er niks naast de diamond/boulder naar beneden valt
                                    if (hitleftup.collider != null || hitrightup.collider != null ||
                                        hitleftup.collider == null && hitrightup.collider == null)
                                    {
                                        //kans berekening om naar links of rechts te vallen
                                        leftorright = random.Next(-1, 2);
                                        if (leftorright <= 0)
                                        {
                                            Vector2 Newposition = new Vector2(gameObject.transform.position.x - 1,
                                                gameObject.transform.position.y);
                                            gameObject.transform.position = Newposition;
                                            moved = true;
                                        }

                                        if (leftorright >= 1)
                                        {
                                            Vector2 Newposition = new Vector2(gameObject.transform.position.x + 1,
                                                gameObject.transform.position.y);
                                            gameObject.transform.position = Newposition;
                                            moved = true;
                                        }
                                    }
                                }
                            }

                            //If right raycast is empty and left raycast is not empty
                            if (hitright.collider == null && hitleft.collider != null && hitrightup.collider == null &&
                                moved == false)
                            {
                                //If rightdown is empty, checken of er ruimte is om te vallen
                                if (hitrightdown.collider == null)
                                {
                                    Vector2 Newposition = new Vector2(gameObject.transform.position.x + 1,
                                        gameObject.transform.position.y);
                                    gameObject.transform.position = Newposition;
                                    moved = true;
                                }
                            }

                            //If left raycast is empty and right raycast is not empty
                            if (hitleft.collider == null && hitright.collider != null && hitleftup.collider == null &&
                                moved == false)
                            {
                                if (hitleftdown.collider == null)
                                {
                                    Vector2 Newposition = new Vector2(gameObject.transform.position.x - 1,
                                        gameObject.transform.position.y);
                                    gameObject.transform.position = Newposition;
                                    moved = true;
                                }
                            }
                        }
                    }
                }

                moved = false;
                leftorright = 0;
                timer = 0.15f;
            }
        }
    }
    
}

