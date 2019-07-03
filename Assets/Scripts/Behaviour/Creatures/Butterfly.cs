using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Helper_Scripts;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviour.Creatures
{
    public class ButterflyCell
    {
        private TileBase butterfly;
        private Vector2Int position;
        private Vector2Int flyDirection;
        private int faceDirection;
        private int lastWall;
        private bool canMove;

        public ButterflyCell(TileBase butterfly, Vector2Int position)
        {
            this.butterfly = butterfly;
            this.position = position;
            flyDirection = position;
            faceDirection = 1;
            lastWall = 0;
            canMove = false;
        }

        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        public TileBase Butterfly
        {
            get => butterfly;
            set => butterfly = value;
        }

        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }

        public Vector2Int FlyDirection
        {
            get => flyDirection;
            set => flyDirection = value;
        }

        public int FaceDirection
        {
            get => faceDirection;
            set => faceDirection = value;
        }

        public int LastWall
        {
            get => lastWall;
            set => lastWall = value;
        }
    }


    public class Butterfly : MonoBehaviour
    {
        public GridInfoRetriever gridInfo;
        public GameObject explosion;
        public float moveSpeed;
        public bool isMoving;
        public bool mustMove;
        public Tilemap butterflyTilemap;

        public List<ButterflyCell> butterflyCollection;
        public Dictionary<Vector2Int, string> tiles;
        public Dictionary<Vector2Int, string> neighbourTiles;
        public List<WallDirection> wallDirection;
        public List<WallDirection> moveDirection;

        public List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down
        };

        private Tilemap[] tilemaps;
        private GameObject[] gameObjects;
        public int butterflyCount;

        private void Start()
        {
            tilemaps = FindObjectsOfType<Tilemap>();
            gameObjects = FindObjectsOfType<GameObject>();
            isMoving = false;
            mustMove = false;
            butterflyCollection = new List<ButterflyCell>();
            tiles = new Dictionary<Vector2Int, string>();

            butterflyTilemap = gameObject.GetComponent<Tilemap>();
            neighbourTiles = new Dictionary<Vector2Int, string>();
            wallDirection = new List<WallDirection>();
            moveDirection = new List<WallDirection>
            {
                new WallDirection(0, Vector2Int.left),
                new WallDirection(1, Vector2Int.up),
                new WallDirection(2, Vector2Int.right),
                new WallDirection(3, Vector2Int.down)
            };

            UpdateGridInfo();
            GetButterflies();
            GetMoveDirection();
            butterflyCount = butterflyCollection.Count;
        }

        private void Update()
        {
            
            
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(Move());
            }
            butterflyCount = butterflyCollection.Count;
        }

        public IEnumerator Move()
        {
            int i = 0;
            //bool isSame = false;
            // update the grid info
            UpdateGridInfo();
            GetMoveDirection();
            // get tiles that have to be moved
            foreach (var butterfly in butterflyCollection)
            {
                if (butterfly.CanMove)
                {
                    butterflyTilemap.SetTile(ConvertToVector3(butterfly.Position), null);
                    butterflyTilemap.SetTile(ConvertToVector3(butterfly.FlyDirection), butterfly.Butterfly);
                    //todo: all animations and other scripts go here
                    //==== Put your other useless stuff here ====//
                    // check if fireflies finished moving
                    if (butterflyTilemap.HasTile(ConvertToVector3(butterfly.FlyDirection)))
                    {
                        // update firefly location
                        butterfly.Position = butterfly.FlyDirection;
                        butterfly.CanMove = false;
                        i++;
                    }
                }
            }

            // when all fireflies have finished moving, start another cycle
            if (i == butterflyCollection.Count)
            {
                mustMove = false;
            }

            //waits for moveSpeed amount of seconds till next movement
            yield return new WaitForSeconds(moveSpeed);
            if (!mustMove)
            {
                isMoving = false;
            }
        }

        public void GetButterflies()
        {
            for (int n = butterflyTilemap.cellBounds.xMin; n < butterflyTilemap.cellBounds.xMax; n++)
            {
                for (int p = butterflyTilemap.cellBounds.yMin; p < butterflyTilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = new Vector3Int(n, p, 0);
                    Vector2Int location = new Vector2Int(localPlace.x, localPlace.y);

                    if (butterflyTilemap.HasTile(localPlace))
                    {
                        butterflyCollection.Add(
                            new ButterflyCell(
                                butterflyTilemap.GetTile(localPlace),
                                location
                            )
                        );
                    }
                }
            }
        }

        // updates all tile information on teh grid
        public void UpdateGridInfo()
        {
            tiles = gridInfo.GetTilemaps(tilemaps, gameObjects);
        }

        public Vector3Int ConvertToVector3(Vector2Int v2)
        {
            return new Vector3Int(v2.x, v2.y, 0);
        }
        public Vector2Int ConvertToVector2(Vector3Int v3)
        {
            return new Vector2Int(v3.x, v3.y);
        }

        public void DestroyButterfly(Vector3Int position)
        {
            foreach (var firefly in butterflyCollection)
            {
                if (firefly.Position == ConvertToVector2(position))
                {
                    butterflyCollection.Remove(firefly);
                    butterflyTilemap.SetTile(ConvertToVector3(firefly.Position), null);
                }
            }
        }

        // get neighbour tiles for each firefly
        public void GetNeighbourTiles(Vector2Int position, int lastDir)
        {
            neighbourTiles.Clear();

            string tileName;


            // depending on this, choose the priority direction


            foreach (var direction in directions)
            {
                Vector2Int tilePosition = new Vector2Int(position.x + direction.x, position.y + direction.y);
                try
                {
                    tileName = tiles[tilePosition];
                }
                catch (KeyNotFoundException)
                {
                    tileName = "Void";
                }

                neighbourTiles.Add(new Vector2Int(tilePosition.x, tilePosition.y), tileName);
            }
        }

        public void GetMoveDirection()
        {
            foreach (var butterfly in butterflyCollection)
            {
                //bool hasDirection = false;
                Vector2Int pos = butterfly.Position;
                GetNeighbourTiles(pos, butterfly.FaceDirection);
                int i = 0;
                wallDirection.Clear();
                moveDirection.Clear();

                foreach (var tile in neighbourTiles)
                {
                    bool isWall = false;
                    switch (tile.Value)
                    {
                        case "Player":
                            isWall = false;
                            break;
                        case "Void":
                            isWall = false;
                            break;
                        case "Amoeba":
                            isWall = false;
                            break;
                        default:
                            isWall = true;
                            break;
                    }

                    moveDirection.Add(new WallDirection(i, tile.Key));
                    if (isWall)
                    {
                        wallDirection.Add(new WallDirection(i, tile.Key));
                    }
                    i++;
                }

                int rotationCounter = 0;
                // if there is a wall
                if (wallDirection.Count != 0)
                {
                    while (true)
                    {
                        // firefly is stuck
                        if (rotationCounter > 4)
                        {
                            break;
                        }
                        // if there is no wall to te 
                        if (!checkCollision(Rotate(butterfly.FaceDirection), butterfly.Position))
                        {
                            butterfly.FaceDirection = Rotate(butterfly.FaceDirection);
                            break;
                        }

                        if (checkCollision(butterfly.FaceDirection, butterfly.Position))
                        {
                            butterfly.FaceDirection = CounterRotate(butterfly.FaceDirection);
                            rotationCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // move forward
                    if (rotationCounter < 4)
                    {
                        butterfly.FlyDirection = moveDirection[butterfly.FaceDirection].Coords;
                        butterfly.CanMove = true;
                    }
                    // firefly is stuck
                    else
                    {
                        butterfly.CanMove = false;
                    }
                }
                // go counter clockwise
                else
                {
                    butterfly.FlyDirection = moveDirection[Rotate(butterfly.FaceDirection)].Coords;
                    butterfly.CanMove = true;
                }
            }
        }

        public bool checkCollision(int dir, Vector2Int position)
        {
            string tileName;
            bool isWall;

            Vector2Int direction = directions[dir];

            Vector2Int tilePosition = new Vector2Int(position.x + direction.x, position.y + direction.y);
            try
            {
                tileName = tiles[tilePosition];
            }
            catch (KeyNotFoundException)
            {
                tileName = "Void";
            }

            switch (tileName)
            {
                case "Player":
                    isWall = false;
                    DrawExplosion(tilePosition);
                    Destroy(GameObject.FindWithTag("Player"));
                    break;
                case "Void":
                    isWall = false;
                    break;
                case "Amoeba":
                    isWall = false;
                    DestroyButterfly(ConvertToVector3(tilePosition));
                    DrawExplosion(tilePosition);
                    // explode
                    break;
                default:
                    isWall = true;
                    break;
            }
            return isWall;
        }

        public int Rotate(int dir)
        {
            if (dir == 3)
            {
                return 0;
            }
            return dir + 1;
        }

        public int CounterRotate(int dir)
        {
            if (dir == 0)
            {
                return 3;
            }
            return dir - 1;
        }
        
        public void DrawExplosion(Vector2Int tilePosition)
        {
            //Draw 3x3 explosion grid
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up + Vector3.left, Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up, Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up + Vector3.right, Quaternion.identity);

            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.left, Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)), Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.right, Quaternion.identity);

            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.down + Vector3.left, Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition))+ Vector3.down, Quaternion.identity);
            Instantiate(explosion, butterflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.down + Vector3.right, Quaternion.identity);
        }

    }
}