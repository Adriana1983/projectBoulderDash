using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class CreatureBehaviour : MonoBehaviour
{
    protected RaycastHit2D wallUp;
    protected RaycastHit2D wallDown;
    protected RaycastHit2D wallLeft;
    protected RaycastHit2D wallRight;

    private float speed;
    [SerializeField]private LayerMask blockingLayer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        speed = 10.0f;
    }

    // Update is called once per frame

    protected bool Move(float x, float y, out RaycastHit2D hit)
    {
        Vector2 currentPos = new Vector2(Mathf.Round(transform.position.x * 2) / 2, Mathf.Round(transform.position.y * 2) / 2);
        Vector2 moveTo = currentPos + new Vector2(x, y);
        hit = Physics2D.Linecast(currentPos, moveTo, blockingLayer);

        if (IsAttached() && !IsBlocked(hit))
        {
            StartCoroutine(SmoothMovement(moveTo));
            return true;
        }
        else if (IsBlocked(hit))
        {
            FindNewDirection(hit);
            return true;
        }
        else
        {
            return false; 
        }
    }

    protected bool IsBlocked(RaycastHit2D hit)
    {
        if (hit.transform == null)
        {
            return false;
        }
        else
        {
            Debug.Log("Blocked");
            return true;
        }
    }

    protected IEnumerator SmoothMovement(Vector3 moveTo)
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
    }

    protected virtual void AttemptMove(float x, float y)
    {
        wallUp = Physics2D.Linecast(transform.position, Vector2.up, blockingLayer);
        wallDown = Physics2D.Linecast(transform.position, Vector2.down, blockingLayer);
        wallLeft = Physics2D.Linecast(transform.position, Vector2.left, blockingLayer);
        wallRight = Physics2D.Linecast(transform.position, Vector2.right, blockingLayer);

        bool canMove = Move(x, y, out RaycastHit2D hit);

        if (hit.transform == null)
        {
            return;
        }

        if (!canMove)
        {
            OnChangeDirection();
        }
    }

    protected abstract void OnChangeDirection();
    protected abstract void FindNewDirection(RaycastHit2D hit);
    protected abstract int ApplyDirection(int direction, out float horizontal, out float vertical);
    protected abstract bool IsAttached();
}
