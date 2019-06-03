using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public class Diamond : MonoBehaviour
{
    private float timer = 1f;
    Random random = new Random();
    private bool moved;

    void Update()
    {
        int leftorright = 0;
        Vector2 positionrayup = new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y);
        Vector2 positionraydown = new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y - 1f);
        Vector2 positionrayright = new Vector2(gameObject.transform.position.x + 1f, gameObject.transform.position.y - 0.5f);
        Vector2 positionrayleft = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
        RaycastHit2D hitup = Physics2D.Raycast(positionrayup, Vector2.up, 1);
        RaycastHit2D hitdown = Physics2D.Raycast(positionraydown, Vector2.down, 1);
        RaycastHit2D hitright = Physics2D.Raycast(positionrayright, Vector2.right, 1);
        RaycastHit2D hitleft = Physics2D.Raycast(positionrayleft, Vector2.left, 1);
        
        Debug.DrawRay(positionrayup, Vector2.up, Color.red);
        Debug.DrawRay(positionraydown, Vector2.down, Color.blue);
        Debug.DrawRay(positionrayright, Vector2.right, Color.magenta);
        Debug.DrawRay(positionrayleft, Vector2.left, Color.yellow);
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            if (hitdown.collider != null)
            {
                if (hitdown.collider.CompareTag("Dirt"))
                {
                }
                if (hitdown.collider.CompareTag("Player"))
                {
                }
               
                if (hitdown.collider.CompareTag("Diamond"))
                {                    
                    if (hitright.collider == null && hitleft.collider == null && moved == false)
                    {
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

                    if (hitright.collider == null && hitleft.collider != null && moved == false)
                    {
                        Vector2 Newposition = new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                        gameObject.transform.position = Newposition;
                        moved = true;
                    }
                    if (hitleft.collider == null && hitright.collider != null && moved == false)
                    {
                        Vector2 Newposition = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                        gameObject.transform.position = Newposition;
                        moved = true;
                    }
                }
            }

            if (hitdown.collider == null && moved == false)
            {
                Vector2 Newposition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                gameObject.transform.position = Newposition;
                moved = true;
            }
            moved = false;
            timer = 1f;
        }
    }
}

