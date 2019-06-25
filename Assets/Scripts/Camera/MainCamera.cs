using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector3;

namespace MainCamera
{
    public class MainCamera : MonoBehaviour
    {
        private GameObject mainCamera;
        private GameObject player;
        private GameObject playerSpawn;

        [SerializeField] private Vector3 minCameraPosition;
        [SerializeField] private Vector3 maxCameraPosition;

        private float mapWidth;
        private float mapHeight;

        private List<Vector3> cameraBounds = new List<Vector3>();

        void Awake()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            playerSpawn = GameObject.FindGameObjectWithTag("Door");

            mapWidth = (int)maxCameraPosition.x;
            mapHeight = (int)maxCameraPosition.y;

            Camera.main.orthographicSize = mapHeight / 4;
            Camera.main.aspect = (mapWidth / 4) / (mapHeight / 4);
        }
        void Start()
        {
            InitializeCameraBounds();
            mainCamera.transform.position = WhereIs(playerSpawn.transform.position);
            Debug.Log(IsInCoordinate(new Vector3(1.0f, 1.0f, 0), cameraBounds[0], cameraBounds[1]));
        }

        void Update()
            {
            if(PlayerHasSpawned())
            {
                Debug.Log("Player is in " + WhereIs(player.transform.position));
                mainCamera.transform.position = WhereIs(player.transform.position);
            }
        }

        private void InitializeCameraBounds()
        {
            for (int y = 1; y < 4; y++)
            {
                for (int x = 1; x < 4; x++)
                {
                    // subtracts 0.5f to prevent seeing half blocks on the bounds
                    cameraBounds.Add(new Vector3((mapWidth / 4) * x -0.5f, (mapHeight / 4) * y - 0.5f, -10));
                }
            }
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
            if(item.x >= isInMin.x && item.x <= isInMax.x && item.y >= isInMin.y && item.y <= isInMax.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Vector3 WhereIs(Vector3 waldo)
        {
            Vector3 newPos = waldo;
            foreach (Vector3 bound in cameraBounds)
            {
                if(IsInCoordinate(
                    waldo, new Vector3(bound.x - 10, bound.y - 5.5f, bound.z), new Vector3(bound.x, bound.y, bound.z)))
                {
                    newPos = bound;
                    Debug.Log("Waldo is in " + waldo);
                }
                else
                {
                    Debug.Log("Waldo is not in " + bound);
                    continue;
                }
            }
            return new Vector3(newPos.x, newPos.y, -10);
        }
    }
}
