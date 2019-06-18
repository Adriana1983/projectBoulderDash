using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

namespace Camera
{
    public class MainCamera : MonoBehaviour
    {
        public Transform player;
        public Transform mainCamera;
        public Vector3 offset;
        private Vector3 startingPosition;
        public bool isLocked;
        private float cameraX;
        private float cameraY;
        private Vector3 cameraBounds;

        public float dampTime = 0.15f;
        private Vector3 velocity = Vector3.zero;

        void Start()
        {
            startingPosition = transform.position;
//            cameraX = OrthographicBounds(GetComponent<UnityEngine.Camera>()).x;
//            cameraY = OrthographicBounds(GetComponent<UnityEngine.Camera>()).y;
//            cameraBounds = OrthographicBounds(GetComponent<UnityEngine.Camera>());
            
        }

        void Update()
        {
            //toggle spacebar for locked/unlocked camera
            if (Input.GetKeyDown("space"))
            {
                isLocked = !isLocked;
            }
            if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene("MainScene");
            
           

            //            RaycastHit2D hitleft = Physics2D.Raycast(transform.position,  Vector2.left, -cameraX);
            //            RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right + cameraBounds, cameraX);
            //            RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down + cameraBounds, -cameraY);
            //            RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up + cameraBounds, cameraY);
            //            
            //            if (hitleft.collider != null && hitright.collider.CompareTag("CameraBounds") )
            //            {
            //               Debug.Log(hitleft.collider + "left");
            //            }
            //            if (hitright.collider != null && hitright.collider.CompareTag("CameraBounds"))
            //            {
            //                Debug.Log(hitright.collider + "right");
            //            }
            //            if (hitdown.collider != null && hitdown.collider.CompareTag("CameraBounds"))
            //            {
            //                Debug.Log(hitdown.collider + "down");
            //            }
            //            if (hitup.collider != null && hitup.collider.CompareTag("CameraBounds"))
            //            {
            //                Debug.Log(hitup.collider + "up");
            //            }
        }

        public void LateUpdate()
        {
            Vector3 point = GetComponent<UnityEngine.Camera>().WorldToViewportPoint(mainCamera.position);
            Vector3 delta = mainCamera.position - GetComponent<UnityEngine.Camera>()
                                .ViewportToWorldPoint(new Vector3(0.5f, 0.5f,
                                    point.z)); //(new Vector3(0.5, 0.5, point.z));
            
            if (isLocked)
            {
                Vector3 destination =
                    new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z - 10) + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
            else
            {
                Vector3 destination = startingPosition + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }

        public static Vector3 OrthographicBounds(UnityEngine.Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
           
            return new Vector3(cameraHeight * screenAspect, cameraHeight, 0);
        }
    }
}
