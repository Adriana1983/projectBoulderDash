using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Helper_Scripts;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviour.Creatures
{
    public enum Direction
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3
    }
    public class FireflyCell
    {
        private TileBase firefly;
        private Vector2Int position;
        private Vector2Int flyDirection;
        private Direction lastDirection;
        private bool canMove;
        
        public FireflyCell(TileBase firefly, Vector2Int position)
        {
            this.firefly = firefly;
            this.position = position;
            flyDirection = position;
            lastDirection = Direction.Left;
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
        
        public Direction LastDirection
        {
            get => lastDirection;
            set => lastDirection = value;
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

        public float moveSpeed;
        public bool isMoving;
        public bool mustMove;
        public Tilemap fireflyTilemap;
        
        private List<Vector3> allowedDirections;
        public List<FireflyCell> fireflyCollection;
        public Dictionary<Vector2Int, string> tiles;
        public Dictionary<Vector2Int, string> neighbourTiles;
        public List<WallDirection> wallDirection;
        public List<WallDirection> moveDirection;
        
        
        private Tilemap[] tilemaps;
        private GameObject[] gameObjects;
        private void Start()
        {
            tilemaps = FindObjectsOfType<Tilemap>();
            gameObjects =  FindObjectsOfType<GameObject>() ;
            isMoving = false;
            mustMove = false;
            fireflyCollection = new List<FireflyCell>();
            tiles = new Dictionary<Vector2Int, string>();
           
            fireflyTilemap = gameObject.GetComponent<Tilemap>();
            neighbourTiles = new Dictionary<Vector2Int, string>();
            wallDirection = new List<WallDirection>();
            
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

        public void DestroyFirefly(Vector2Int position)
        {
            foreach (var firefly in fireflyCollection)
            {
                if (firefly.Position == position)
                {
                    fireflyCollection.Remove(firefly);
                    fireflyTilemap.SetTile(ConvertToVector3(firefly.Position), null);
                    //todo: initiate explosion here
                }
            }
        }
        
        // get neighbour tiles for each firefly
        public void GetNeighbourTiles(Vector2Int position)
        {
            neighbourTiles.Clear();
            
            string tileName;
            
            // depending on this, choose the priority direction
            Vector2Int[] directions = 
            {
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down
            };
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
                GetNeighbourTiles(pos);
                int iterator = 0;
                wallDirection.Clear();
                moveDirection.Clear();
                
                // get directions which firefly can move
                foreach (var tile in neighbourTiles)
                {
                    bool canMove = false;
                    switch (iterator)
                    {
                        // left
                        case 0:
                            switch (tile.Value)
                            {
                                case "Player":
                                    // explode?
                                    break;
                                case "Void":
                                    canMove = true;
                                    break;
                                case "Amoeba":
                                    // explode
                                    break;
                            }
                            break;
                        // up
                        case 1:
                            switch (tile.Value)
                            {
                                case "Player":
                                    // explode?
                                    break;
                                case "Void":
                                    canMove = true;
                                    break;
                                case "Amoeba":
                                    // explode
                                    break;
                            }
                            break;
                        // right
                        case 2:
                            switch (tile.Value)
                            {
                                case "Player":
                                    // explode?
                                    break;
                                case "Void":
                                    canMove = true;
                                    break;
                                case "Amoeba":
                                    // explode
                                    break;
                            }
                            
                            break;
                        // down
                        case 3:
                            switch (tile.Value)
                            {
                                case "Player":
                                    // explode?
                                    break;
                                case "Void":
                                    canMove = true;
                                    break;
                                case "Amoeba":
                                    // explode
                                    break;
                            }
                            break;
                    }
                    if (canMove)
                    {
                        moveDirection.Add(new WallDirection(iterator, tile.Key));
                    }
                    else
                    {
                        wallDirection.Add(new WallDirection(iterator, tile.Key));
                    }
                    iterator++;
                }
                
                if (wallDirection.Count > 0)
                {
                    // check move directions depending on left wall, left wall is always at index 0
                    // first, reorder the walls so that the left wall is at index 0 depending on direction the firefly is facing
                    int 
                    if (firefly.LastDirection == Direction.Left)
                    {
                        
                    }
                    var order = Reorder();
                    wallDirection = order.Select(i => wallDirection[i]).ToList();
                    firefly.FlyDirection = wallDirection.First().Coords;
                    firefly.CanMove = true;
                    firefly.LastDirection = (Direction) wallDirection.First().Direction;
                }
                else
                {
                    firefly.FlyDirection = moveDirection.First().Coords;
                    firefly.CanMove = true;
                    firefly.LastDirection = Direction.Left;
                }
            }
        }
        
        public List<int> Reorder(int direction)
        {
            var list = new List<int>{0, 1, 2, 3};
            return list.Skip(direction).Concat(list.Take(direction).Reverse()).ToList();
        }
    }
}
