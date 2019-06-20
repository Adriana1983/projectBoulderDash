using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireflyBehaviour : CreatureBehaviour
{
    [SerializeField] float horizontal; // Remove serializefield
    [SerializeField] float vertical; // Remove serializefield

    private enum WallDirections
    {
        Down = 0, // moves right
        Right = 1, // moves up
        Up = 2, // moves right
        Left = 3 // moves down
    }
    [SerializeField] private int wallDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        wallDirection = (int)WallDirections.Down; // starts off moving right
        ApplyDirection(wallDirection, out horizontal, out vertical); // Initial applydirection
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove(horizontal, vertical);
        }
    }

    protected override void AttemptMove(float x, float y)
    {   
        base.AttemptMove(x, y);
    }

    protected override void OnChangeDirection()
    {
        if (wallRight.transform == null)
        {
            wallDirection = (int)WallDirections.Right;
        }
        if (wallUp.transform == null)
        {
            wallDirection = (int)WallDirections.Up;
        }
        if (wallLeft.transform == null)
        {
            wallDirection = (int)WallDirections.Left;
        }
        if (wallDown.transform == null)
        {
            wallDirection = (int)WallDirections.Down;
        }
        ApplyDirection(wallDirection, out horizontal, out vertical);
    }

    protected override int ApplyDirection(int direction, out float horizontal, out float vertical)
    {
        switch (direction)
        {
            case 0: // right
                horizontal = -0.5f;
                vertical = 0;
                break;
            case 1: // up
                horizontal = 0;
                vertical = -0.5f;
                break;
            case 2: // left
                horizontal = 0.5f;
                vertical = 0;
                break;
            case 3: // down
                horizontal = 0;
                vertical = 0.5f;
                break;
            default: // default is right
                horizontal = -0.5f;
                vertical = 0;
                break;
        }
        return direction;
    }

    protected override bool IsAttached()
    {
        if (wallDirection == 0 && wallDown.transform == null)
        {
            return false;
        }
        else if (wallDirection == 1 && wallRight.transform == null)
        {
            return false;
        }
        else if (wallDirection == 2 && wallUp.transform == null)
        {
            return false;
        }
        else if (wallDirection == 3 && wallLeft.transform == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected override void FindNewDirection(RaycastHit2D hit)
    {
        if (wallDirection >= 4)
        {
            wallDirection = 0;
        }
        else
        {
            wallDirection++;
        }
    }
}
