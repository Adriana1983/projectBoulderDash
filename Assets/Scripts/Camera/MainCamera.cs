using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;
using Behaviour.Player;

namespace MainCamera
{
    public class MainCamera : MonoBehaviour
    {
        private GameObject mainCamera;
        private GameObject player;
        private GameObject playerSpawn;
        private Movement movementScript;

        [SerializeField] private Vector3 minCameraPosition;
        [SerializeField] private Vector3 maxCameraPosition;
        private Vector3 currentPos;

        private float mapWidth;
        private float mapHeight;

        private bool foundPos;

        private List<Vector3> cameraBounds = new List<Vector3>();

        void Awake()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            playerSpawn = GameObject.FindGameObjectWithTag("Door");

            mapWidth = maxCameraPosition.x;
            mapHeight = maxCameraPosition.y;

            Camera.main.orthographicSize = 5.5f;
        }
        void Start()
        {
            InitializeCameraBounds();
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, WhereIs(playerSpawn.transform.position), 10 * Time.deltaTime);
            currentPos = new Vector3(0, 0, -10);
            foundPos = false;
        }

        void Update()
        {
            if(PlayerHasSpawned())
            {
                movementScript = player.GetComponent<Movement>();
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, WhereIs(player.transform.position), 10 * Time.deltaTime);
            }
        }


        private void InitializeCameraBounds()
        {
            // TODO find way to automate this
            cameraBounds.Add(new Vector3(9.5f, 16, -10));
            cameraBounds.Add(new Vector3(9.5f, 10.5f, -10));
            cameraBounds.Add(new Vector3(9.5f, 5, -10));
            cameraBounds.Add(new Vector3(19.5f, 16, -10));
            cameraBounds.Add(new Vector3(19.5f, 10.5f, -10));
            cameraBounds.Add(new Vector3(19.5f, 5, -10));
            cameraBounds.Add(new Vector3(29.5f, 16, -10));
            cameraBounds.Add(new Vector3(29.5f, 10.5f, -10));
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
            int moveDirection = movementScript.animationDirection;
            Vector3 waldoPos = new Vector3(9.5f, 16, -10);
            foreach(Vector3 bound in cameraBounds)
            {
                if (IsInCoordinate(
                    waldo, new Vector3(bound.x - 6, bound.y - 3, -10), new Vector3(bound.x + 6, bound.y + 3, -10)))
                {
                    if (moveDirection == 1 || moveDirection == 3)
                    {
                        waldoPos = new Vector3(waldo.x, bound.y, -10);
                        break;
                    }
                    else if (moveDirection == 2 || moveDirection == 4)
                    {
                        waldoPos = new Vector3(bound.x, waldo.y, -10);
                        break;
                    }
                }
                else
                {
                    waldoPos = new Vector3(waldo.x, waldo.y, -10);
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
