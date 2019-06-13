using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBoulder : MonoBehaviour
{
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.up, 2);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Boulder"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            if (hit2.collider != null)
            {
                if (hit2.collider.CompareTag("Boulder"))
                {
                    Destroy(hit2.collider.gameObject);
                }
            }
        }
    }
}
