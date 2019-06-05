using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    public GameObject Boulder;
    public GameObject Diamond;
    private bool activatedDiamond;
    private bool activatedBoulder;
    private int activeTime;
    private float Timer = 0.15f;
    private float Timer2 = 0.30f;

    private void Update()
    {
        Vector2 positionrayup = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f);
        Vector2 positionrayupup = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.6f);

        RaycastHit2D hitup = Physics2D.Raycast(positionrayup, Vector2.up, 0.5f);
        RaycastHit2D hitupup = Physics2D.Raycast(positionrayupup, Vector2.up, 0.5f);
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            if (hitup.collider == null && hitupup.collider != null)
            {
                if (hitupup.collider.CompareTag("Diamond"))
                {
                    activatedDiamond = true;
                }
            }

            if (activatedDiamond)
            {
                bool falling = GetComponent<Diamond>().Falling;
                print(falling);
                if (falling == true)
                {
                    {
                        Destroy(hitup.collider.gameObject);
                        Instantiate(Boulder,
                            new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1),
                            Quaternion.identity);
                    }
                }
            }

        }

        Timer = 0.15f;
    }
}


