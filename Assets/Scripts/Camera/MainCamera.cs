using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class MainCamera : MonoBehaviour
{
    public Transform player;
    public Transform mainCamera;
    public Vector3 offset;
    private Vector3 startingPosition;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
   
 
    // Update is called once per frame
    void Update () 
    {
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(mainCamera.position);
        Vector3 delta = mainCamera.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        
        if (Input.GetKey("space"))
        {
            Vector3 destination = new Vector3 (player.position.x + offset.x, player.position.y + offset.y, offset.z-10) + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else
        {
            Vector3 destination = startingPosition + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
     
    }
    
    private void Start()
    {
        startingPosition = transform.position;
    }
}
