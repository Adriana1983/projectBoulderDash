using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Firefly_behavior : MonoBehaviour
{
    public Vector3 targetPos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
         speed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector3.left, 0.5f);
        RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector3.right, 0.5f);
        RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector3.up, 0.5f);
        RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector3.down, 0.5f);

        if (hitup.collider != null && hitleft.collider == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, speed * Time.deltaTime);
        }
        else if (hitleft.collider != null && hitdown.collider == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, speed * Time.deltaTime);
        }
        else if (hitdown.collider != null && hitright.collider == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, speed * Time.deltaTime);
        }
        else if (hitright.collider != null && hitup.collider == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, speed * Time.deltaTime);
        }
    }
}
