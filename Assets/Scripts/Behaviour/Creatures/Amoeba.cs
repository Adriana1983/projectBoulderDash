using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Behaviour.Objects;
using Helper_Scripts;
using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviour.Creatures
{
    public enum GrowState
    {
        Sleeping,
        Slow,
        Fast
    }

    public class AmoebaCell
    {
        private TileBase amoeba;
        private TileBase amoebaPlaceholder;
        private GrowState state;
        private Vector2Int position;
        private List<Vector2Int> growPostion;
        private int growCycle;
        
        public AmoebaCell(TileBase amoeba, TileBase amoebaPlaceholder, Vector2Int position, GrowState state)
        {
            this.amoeba = amoeba;
            this.amoebaPlaceholder = amoebaPlaceholder;
            this.state = state;
            this.position = position;
            growCycle = 0;
        }
        public TileBase Amoeba
        {
            get => amoeba;
            set => amoeba = value;
        }

        public TileBase AmoebaPlaceholder
        {
            get => amoebaPlaceholder;
            set => amoebaPlaceholder = value;
        }

        public GrowState State
        {
            get => state;
            set => state = value;
        }

        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }
        public List<Vector2Int> GrowPostion
        {
            get => growPostion;
            set => growPostion = value;
        }
        
        public int GrowCycle
        {
            get => growCycle;
            set => growCycle = value;
        }
    }
    
    public class Amoeba : MonoBehaviour
    {
        public GridInfoRetriever gridInfo;
        
        public Vector3 targetPos;
        public bool mustGrow;
        public bool isGrowing;
        public int maxSize;
        public int amoebaCount;

        public LayerMask layer;
        public Tilemap amoebaTilemap;
        public float nextGrow;
        public float growSpeed;

        private List<Vector3> allowedDirections;
        public List<AmoebaCell> amoebaCollection;
        public List<Vector2Int> growDirections;
       

        public BoundsInt bounds;
        public TileBase[] amoebaTiles;
        public TileBase[] allTiles;
        public Dictionary<Vector2Int, string> tiles;
        public Dictionary<Vector2Int, string> neighbourTiles;

        private void Start()
        {
            Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();
            targetPos = transform.position;
            isGrowing = false;
            mustGrow = false;
            amoebaCollection = new List<AmoebaCell>();
            growDirections = new List<Vector2Int>();
            tiles = new Dictionary<Vector2Int, string>();
            amoebaTilemap = gameObject.GetComponent<Tilemap>();
            neighbourTiles = new Dictionary<Vector2Int, string>();
            // get all tiles
            tiles = gridInfo.GetTilemaps(tilemaps);
            bounds = amoebaTilemap.cellBounds;
            amoebaTiles = amoebaTilemap.GetTilesBlock(bounds);
            GetMotherCells();
            UpdateGrowLocation();
        }

        private void Update()
        {
            amoebaCount = amoebaCollection.Count;


            // limit grow speed by looking at time
            // this script fires every "growSpeed" seconds
            if (Time.time > nextGrow)
            {
             
//                if (allowedDirections.Count == 0)
//                {
//                    mustGrow = false;
//                }
//                else
//                {
//                    // get random direction to grow for amoeba;
//                    var random = new Random();
//                    int index = random.Next(allowedDirections.Count);
//                    Vector3 direction = allowedDirections[index];
//                    hitPos += direction;
//                    
//                    //Check for collision in requested player direction
//                    RaycastHit2D hit = Physics2D.Raycast(transform.position, hitPos, 1, layer);
//                    Debug.DrawRay(previous, direction, Color.red);
//                    //Did we hit anything?
//                    if (hit.collider != null)
//                    {
//                        //What did we hit?
//                        switch (hit.collider.gameObject.tag)
//                        {
//                            case "Dirt":
//                                //Allow amoeba to grow
//                                mustGrow = true;
//                                targetPos = hitPos;
//                                break;
//                            case "Butterfly":
//                                break;
//                            case "Firefly":
//                                break;
//                            //We hit something else, amoeba cannot grow
//                            default:
//                                mustGrow = false;
//                                break;
//                        }
//                    }
//                    else
//                    {
//                        targetPos = hitPos;
//                        mustGrow = true;
//                    }
//                    
//                    // Can amoeba grow?
//                    if (mustGrow)
//                    {
//                        // Dont allow amoebas to grow more than this
//                        if (amoebaCount < maxSize)
//                        {
//                            // if the position has changed
//                            if (targetPos != previous)
//                            {
//                                Instantiate(GameObject.Find("Amoeba"), targetPos, new Quaternion(0, 0, 0, 0));
//                                //Delete the dirt
//                                if (dirtTileMap != null)
//                                {
//                                    dirtTileMap.SetTile(dirtTileMap.WorldToCell(targetPos), null);
//                                }
//                                isGrowing = true;
//                                // save the new position for next frame
//                                previous = targetPos;
//                            }
//                        }
//                    }
//                }
                nextGrow += growSpeed;
            }

        }

        private void FixedUpdate()
        {
            
        }

        private void LateUpdate()
        {
          
        }
        
        // iterate through all tiles to get amoeba mother cells
        public void GetMotherCells()
        {
            for (int x = 0; x < bounds.size.x; x++) {
                for (int y = 0; y < bounds.size.y; y++) {
                    TileBase tile = amoebaTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        amoebaCollection.Add(
                            new AmoebaCell(
                                tile, 
                                null,
                                // y - 3, cause it offsets the y for some reason
                                new Vector2Int(x, y - 3),
                                GrowState.Sleeping)
                        ); 
                    }
                }
            }
        }

        public void UpdateGrowLocation()
        {
            growDirections.Clear();
            foreach (var amoebaCell in amoebaCollection)
            {
                Vector2Int curentPos = amoebaCell.Position;
                GetNeighbourTiles(curentPos);
                if (neighbourTiles != null)
                {
                    foreach (var tile in neighbourTiles)
                    {
                        if (tile.Value == null)
                        {
                            // store the available locations as Vector2
                            growDirections.Add(tile.Key);
                        }
                    }
                }
            }
        }

        public TileBase GetTile(int x, int y)
        {
            return amoebaTiles[x + y * bounds.size.x];
        }

        public void GetNeighbourTiles(Vector2Int position)
        {
            neighbourTiles.Clear();
            for (int y = 1; y >= -1; y--)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x != 0 || y != 0)
                    {
                       Vector2Int tilePosition = new Vector2Int(position.x + x, position.y + y);
                       neighbourTiles.Add(new Vector2Int(tilePosition.x, tilePosition.y), tiles[tilePosition]);
                       Debug.Log(tilePosition + " has tile" + tiles[tilePosition]);
                    }
                }
            }
        }

        public float GetGrowSpeed(GrowState state)
        {
            float growingSpeed = 0;
            var random = new Random();
            
            switch (state)
            {
                case GrowState.Sleeping:
                    growingSpeed = 0;
                    break;
                case GrowState.Slow:
                    growingSpeed = random.Next(0, 24);
                    break;
                case GrowState.Fast:
                    growingSpeed = random.Next(0, 4);
                    break;
            }

            return growingSpeed;
        }


//        public Vector2 GetRandomGrowDirection()
//        {
//            var random = new Random();
//            int index = random.Next(growPostion.Count);
//            return growPostion[index];
        //}
    }
}