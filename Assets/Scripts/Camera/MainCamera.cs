using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Playables;
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
            cameraX = OrthographicBounds(GetComponent<UnityEngine.Camera>()).x;
            cameraY = OrthographicBounds(GetComponent<UnityEngine.Camera>()).y;
            cameraBounds = OrthographicBounds(GetComponent<UnityEngine.Camera>());
        }

        void Update()
        {
            //toggle spacebar for locked/unlocked camera
            if (Input.GetKeyDown("space"))
            {
                isLocked = !isLocked;
            }
            if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene("MainScene");

            try
            {
                player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
            catch (Exception e)
            {
                Debug.Log("Player not spawned yet");
            }
           
        }

        void LateUpdate()
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
