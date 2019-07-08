using UnityEngine;

public class DeleteBoulderOrDiamond : MonoBehaviour
{
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1);
        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
        }
    }
}


