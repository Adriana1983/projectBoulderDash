using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    public GameObject Boulder;
    public GameObject Diamond;
    private float activeDuration = 5;
    private bool activated;
    private float Timer = 0.15f;

    private void Update()
    {
        Vector2 positionrayup = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f);
        Vector2 positionrayupup = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.6f);

        RaycastHit2D hitup = Physics2D.Raycast(positionrayup, Vector2.up, 0.5f);
        RaycastHit2D hitupup = Physics2D.Raycast(positionrayupup, Vector2.up, 0.5f);
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            if (hitup.collider != null)
            {
                activated = true;
                
                if (activated)
                {
                    activeDuration -= Time.deltaTime;
                    if (activeDuration <= 0)
                    {
                        activated = false;
                    }
                    if (hitup.collider.CompareTag("Diamond"))
                    {
                        bool falling = hitup.collider.gameObject.GetComponent<Diamond>().Falling;
                        if (falling)
                        {
                            Destroy(hitup.collider.gameObject);
                            Instantiate(Boulder,
                                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1),
                                Quaternion.identity);
                        }
                    }
                    if (hitup.collider.CompareTag("Boulder"))
                    {
                        bool falling = hitup.collider.gameObject.GetComponent<Diamond>().Falling;
                        if (falling)
                        {
                            Destroy(hitup.collider.gameObject);
                            Instantiate(Diamond,
                                new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1),
                                Quaternion.identity);
                        }
                    }
                }
                else
                {
                    print("False");
                }
                
            }

            Timer = 0.15f;
        }
    }
}


