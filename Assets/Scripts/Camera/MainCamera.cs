using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;
using Behaviour.Player;
using System;

namespace MainCamera
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private GameObject mainCamera;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject playerSpawn;
        private Movement movementScript;

        private Vector3 minCameraPosition = new Vector3(9.5f, 5, -10);
        private Vector3 maxCameraPosition = new Vector3(29.5f, 16, -10);
        private Vector3 currentPos;

        private float mapWidth;
        private float mapHeight;

        private bool isMoving;

        private List<Vector3> cameraBounds = new List<Vector3>();

        void Awake()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            mapWidth = maxCameraPosition.x;
            mapHeight = maxCameraPosition.y;

            Camera.main.orthographicSize = 6.5f;
        }
        void Start()
        {
            isMoving = false;
            InitializeCameraBounds();
        }

        void Update()
        {
            if(PlayerHasSpawned())
            {
                // movementScript = player.GetComponent<Movement>();
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, WhereIs(player.transform.position), 10 * Time.deltaTime);
            }
            else
            {
                playerSpawn = GameObject.FindGameObjectWithTag("Door");
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, WhereIs(playerSpawn.transform.position), 10 * Time.deltaTime);
                currentPos = WhereIs(playerSpawn.transform.position);
            }
        }


        private void InitializeCameraBounds()
        {
            // TODO find way to automate this
            cameraBounds.Add(new Vector3(9.5f, 16, -10));
            cameraBounds.Add(new Vector3(19.5f, 16, -10));
            cameraBounds.Add(new Vector3(29.5f, 16, -10));
            cameraBounds.Add(new Vector3(9.5f, 10.5f, -10));
            cameraBounds.Add(new Vector3(19.5f, 10.5f, -10));
            cameraBounds.Add(new Vector3(29.5f, 10.5f, -10));
            cameraBounds.Add(new Vector3(9.5f, 5, -10));
            cameraBounds.Add(new Vector3(19.5f, 5, -10));
            cameraBounds.Add(new Vector3(29.5f, 5, -10));
        }

        private bool PlayerHasSpawned()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                return false;
            }
            return true;
        }

        private bool IsInCoordinate(Vector3 item, Vector3 isInMin, Vector3 isInMax)
        {
            if (item.x > isInMin.x && item.y > isInMin.y && item.x < isInMax.x && item.y < isInMax.y)
            {
                return true;
            }
            return false;
        }

        private Vector3 WhereIs(Vector3 waldo)
        {
            Vector3 waldoPos = new Vector3(waldo.x, waldo.y, -10);
            Vector3 oldBound = mainCamera.transform.position;

            float diffWidth = 100; float diffHeight = 100;

            foreach (Vector3 bound in cameraBounds)
            {
                if (Mathf.Abs(diffWidth) > Mathf.Abs(waldo.x - bound.x))
                {
                    diffWidth = waldo.x - bound.x;
                    waldoPos.x = bound.x;
                }
                if (Mathf.Abs(diffHeight) > Mathf.Abs(waldo.y - bound.y))
                {
                    diffHeight = waldo.y - bound.y;
                    waldoPos.y = bound.y;
                }
            }
            if (waldo.x < minCameraPosition.x)
            {
                waldoPos.x = minCameraPosition.x;
            }
            if (waldo.x > maxCameraPosition.x)
            {
                waldoPos.x = maxCameraPosition.x;
            }
            if (waldo.y < minCameraPosition.y) 
            {
                waldoPos.y = minCameraPosition.y;
            }
            if (waldo.y > maxCameraPosition.y)
            {
                waldoPos.y = maxCameraPosition.y;
            }
            return waldoPos;
        }
    }
}
