using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Behaviour.Objects;
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
        
        public AmoebaCell(TileBase amoeba, TileBase amoebaPlaceholder, Vector2Int position, GrowState state)
        {
            this.amoeba = amoeba;
            this.amoebaPlaceholder = amoebaPlaceholder;
            this.state = state;
            this.position = position;
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

    }
    
    public class Amoeba : MonoBehaviour
    {
        public bool isHit;

        public Vector3 targetPos;
        private Vector3 hitPos;
        private Vector3 previous;
        public bool mustGrow;
        public bool isGrowing;
        public int maxSize;
        public int amoebaCount;

        public LayerMask layer;
        public Tilemap dirtTileMap;
        public Tilemap amoebaTilemap;
        public float nextGrow;
        public float growSpeed;

        private List<Vector3> allowedDirections;
        public List<AmoebaCell> amoebaCollection;
        public List<Vector2Int> motherCells;

        public BoundsInt bounds;
        public TileBase[] allTiles;
        public Dictionary<Vector2Int, TileBase> neighbourTiles;

        private void Start()
        {
            targetPos = transform.position;
            previous = targetPos;
            isGrowing = false;
            mustGrow = false;
            amoebaCollection = new List<AmoebaCell>();
            amoebaTilemap = gameObject.GetComponent<Tilemap>();
            neighbourTiles = new Dictionary<Vector2Int, TileBase>();
            bounds = amoebaTilemap.cellBounds;
            allTiles = amoebaTilemap.GetTilesBlock(bounds);
            GetMotherCells();
            foreach (var amoeba in amoebaCollection)
            {
                motherCells.Add(amoeba.Position);
            }
            UpdateGrowLocation();
        }

        private void Update()
        {
            amoebaCount = amoebaCollection.Count;
            hitPos = targetPos;


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
                    TileBase tile = allTiles[x + y * bounds.size.x];
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
            foreach (var amoebaCell in amoebaCollection)
            {
                Vector2Int curentPos = amoebaCell.Position;
                GetNeighbourTiles(curentPos);
                if (neighbourTiles != null)
                {
                    foreach (var tile in neighbourTiles)
                    {
                        string name;
                        if (tile.Value == null)
                        {
                            name = "empty space";
                        }
                        else
                        {
                            
                            name = tile.Value.name;
                        }
                        
                        Debug.Log("Amoeba at " + curentPos + " has neighbour " + name + " at " + tile.Key);
               
                    }
                }
            }
        }

        public TileBase GetTile(int x, int y)
        {
            return allTiles[x + y * bounds.size.x];
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
                        Vector3Int tilePosition = new Vector3Int(position.x + x, position.y + y, 0);
                        neighbourTiles.Add(new Vector2Int(tilePosition.x, tilePosition.y), amoebaTilemap.GetTile(tilePosition));
                    }
                }
            }
        }


//        public Vector2 GetRanomGrowPosition()
//        {
//            var random = new Random();
//            int index = random.Next(growPostion.Count);
//            return growPostion[index];
//        }
    }
}