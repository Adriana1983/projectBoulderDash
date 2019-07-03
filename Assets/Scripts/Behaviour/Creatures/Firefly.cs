﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Behaviour.Objects;
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

    public class FireflyCell
    {
        private TileBase firefly;
        private Vector2Int position;
        private Vector2Int flyDirection;
        private int faceDirection;
        private int lastWall;
        private bool canMove;

        public FireflyCell(TileBase firefly, Vector2Int position)
        {
            this.firefly = firefly;
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

        public TileBase Firefly
        {
            get => firefly;
            set => firefly = value;
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

    public class WallDirection
    {
        private int direction;
        private Vector2Int coords;

        public WallDirection(int direction, Vector2Int coords)
        {
            this.direction = direction;
            this.coords = coords;
        }

        public Vector2Int Coords
        {
            get => coords;
            set => coords = value;
        }

        public int Direction
        {
            get => direction;
            set => direction = value;
        }
    }

    public class Firefly : MonoBehaviour
    {
        public GridInfoRetriever gridInfo;
        public GameObject explosion;
        public float moveSpeed;
        public bool isMoving;
        public bool mustMove;
        public Tilemap fireflyTilemap;

        public List<FireflyCell> fireflyCollection;
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

        private void Start()
        {
            tilemaps = FindObjectsOfType<Tilemap>();
            gameObjects = FindObjectsOfType<GameObject>();
            isMoving = false;
            mustMove = false;
            fireflyCollection = new List<FireflyCell>();
            tiles = new Dictionary<Vector2Int, string>();

            fireflyTilemap = gameObject.GetComponent<Tilemap>();
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
            GetFireflies();
            GetMoveDirection();
        }

        private void Update()
        {
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(Move());
            }
        }

        public IEnumerator Move()
        {
            int i = 0;
            //bool isSame = false;
            // update the grid info
            UpdateGridInfo();
            GetMoveDirection();
            // get tiles that have to be moved
            foreach (var firefly in fireflyCollection)
            {
                if (firefly.CanMove)
                {
                    fireflyTilemap.SetTile(ConvertToVector3(firefly.Position), null);
                    fireflyTilemap.SetTile(ConvertToVector3(firefly.FlyDirection), firefly.Firefly);
                    //todo: all animations and other scripts go here
                    //==== Put your other useless stuff here ====//
                    // check if fireflies finished moving
                    if (fireflyTilemap.HasTile(ConvertToVector3(firefly.FlyDirection)))
                    {
                        // update firefly location
                        firefly.Position = firefly.FlyDirection;
                        firefly.CanMove = false;
                        i++;
                    }
                }
            }

            // when all fireflies have finished moving, start another cycle
            if (i == fireflyCollection.Count)
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

        public void GetFireflies()
        {
            for (int n = fireflyTilemap.cellBounds.xMin; n < fireflyTilemap.cellBounds.xMax; n++)
            {
                for (int p = fireflyTilemap.cellBounds.yMin; p < fireflyTilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = new Vector3Int(n, p, 0);
                    Vector2Int location = new Vector2Int(localPlace.x, localPlace.y);

                    if (fireflyTilemap.HasTile(localPlace))
                    {
                        fireflyCollection.Add(
                            new FireflyCell(
                                fireflyTilemap.GetTile(localPlace),
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

        public void DestroyFirefly(Vector3Int position)
        {
            foreach (var firefly in fireflyCollection)
            {
                if (firefly.Position == new Vector2Int(position.x, position.y))
                {
                    fireflyCollection.Remove(firefly);
                    fireflyTilemap.SetTile(ConvertToVector3(firefly.Position), null);
                }
            }
        }

        // get neighbour tiles for each firefly
        public void GetNeighbourTiles(Vector2Int position, int lastDir)
        {
            neighbourTiles.Clear();

            string tileName;

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
            foreach (var firefly in fireflyCollection)
            {
                //bool hasDirection = false;
                Vector2Int pos = firefly.Position;
                GetNeighbourTiles(pos, firefly.FaceDirection);
                int i = 0;
                wallDirection.Clear();
                moveDirection.Clear();

                foreach (var tile in neighbourTiles)
                {
                    bool isWall = false;
                    switch (tile.Value)
                    {
                        case "Player":
                            // explode?
                            break;
                        case "Void":
                            isWall = false;
                            break;
                        case "Amoeba":
                            // explode
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
                        if (rotationCounter > 4)
                        {
                            break;
                        }
                        if (firefly.FaceDirection == 8 && !checkCollision(0, firefly.Position))
                        {
                            firefly.FaceDirection = 0;
                            break;
                        }
                        if (!checkCollision(CounterRotate(firefly.FaceDirection), firefly.Position))
                        {
                            firefly.FaceDirection = CounterRotate(firefly.FaceDirection);
                            break;
                        }

                        if (checkCollision(firefly.FaceDirection, firefly.Position))
                        {
                            firefly.FaceDirection = Rotate(firefly.FaceDirection);
                            rotationCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (rotationCounter < 4)
                    {
                        firefly.FlyDirection = moveDirection[firefly.FaceDirection].Coords;
                        firefly.CanMove = true;
                    }
                }
                // go counter clockwise
                else
                {
                    firefly.FlyDirection = moveDirection[CounterRotate(firefly.FaceDirection)].Coords;
                    firefly.CanMove = true;
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
                    DestroyFirefly(ConvertToVector3(tilePosition));
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
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up + Vector3.left, Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up, Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.up + Vector3.right, Quaternion.identity);

            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.left, Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)), Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.right, Quaternion.identity);

            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.down + Vector3.left, Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition))+ Vector3.down, Quaternion.identity);
            Instantiate(explosion, fireflyTilemap.GetCellCenterWorld(ConvertToVector3(tilePosition)) + Vector3.down + Vector3.right, Quaternion.identity);
        }
    }
}