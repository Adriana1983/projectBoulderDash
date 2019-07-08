using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using Behaviour.Objects;
using Helper_Scripts;
using JetBrains.Annotations;
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
        private List<Vector2Int> growDirection;
        private int growCycle;
        
        
        public AmoebaCell(TileBase amoeba, TileBase amoebaPlaceholder, Vector2Int position, GrowState state)
        {
            this.amoeba = amoeba;
            this.amoebaPlaceholder = amoebaPlaceholder;
            this.state = state;
            this.position = position;
            growCycle = 0;
            growDirection = new List<Vector2Int>();
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
        public List<Vector2Int> GrowDirection
        {
            get => growDirection;
            set => growDirection = value;
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
        public bool mustGrow;
        public bool isGrowing;
        public int maxSize;
        public int amoebaCount;

        public Tilemap amoebaTilemap;
        public Tilemap dirtTilemap;
        public Tilemap boulderTilemap;
        public Tilemap diamondTilemap;
        public GameObject diamond;
        public GameObject boulder;
        
        public GrowState defaultState;
        public bool isDormant;
        private List<Vector3> allowedDirections;
        public List<AmoebaCell> amoebaCollection;
        public Dictionary<Vector2Int, string> tiles;
        public Dictionary<Vector2Int, string> neighbourTiles;
        public Random random;

        private Tilemap[] tilemaps;
        private GameObject[] gameObjects;

        private void Start()
        {
            tilemaps = FindObjectsOfType<Tilemap>();
            gameObjects =  FindObjectsOfType<GameObject>() ;
            isGrowing = false;
            mustGrow = false;
            amoebaCollection = new List<AmoebaCell>();
            tiles = new Dictionary<Vector2Int, string>();
           
            amoebaTilemap = gameObject.GetComponent<Tilemap>();
            neighbourTiles = new Dictionary<Vector2Int, string>();
            
            random = new Random();
            random = new Random(random.Next());
            
            UpdateGridInfo();
            GetAmoebaCells();
        }

        private void Update()
        {
            amoebaCount = amoebaCollection.Count;

            // if amoeba exceeds the maximum grow size, turn it to boulders
            if (amoebaCount >= maxSize)
            {
                //turn every tile into Boulder GameObject
                foreach (var amoebaCell in amoebaCollection)
                {
                    amoebaTilemap.SetTile(ConvertToVector3(amoebaCell.Position), null);
                    TurnToBoulder(ConvertToVector3(amoebaCell.Position));
                }
                // destroy the script
                Destroy(GetComponent<Amoeba>());
            }
            // if amoeba can't grow but is not max size, turn it to diamonds
            else if (amoebaCount < maxSize && isDormant)
            {
                //turn every tile into Crystal GameObject
                foreach (var amoebaCell in amoebaCollection)
                {
                    amoebaTilemap.SetTile(ConvertToVector3(amoebaCell.Position), null);
                    TurnToDiamond(ConvertToVector3(amoebaCell.Position));
                }
                // destroy the script
                Destroy(GetComponent<Amoeba>());
            }
            else
            {
                if (isGrowing == false)
                {
                    isGrowing = true;
                    StartCoroutine("Grow");
                }
            }
        }

        public void TurnToBoulder(Vector3Int location)
        {
            Vector3 localPlace = boulderTilemap.GetCellCenterWorld(location);
            Instantiate(boulder, localPlace, Quaternion.identity, boulderTilemap.gameObject.transform);
            boulder.layer = LayerMask.NameToLayer("Boulder");
        }
        
        public void TurnToDiamond(Vector3Int location)
        {
            Vector3 localPlace =  boulderTilemap.GetCellCenterWorld(location);
            Instantiate(diamond, localPlace, Quaternion.identity);
            diamond.layer = LayerMask.NameToLayer("Diamond");
        }
        
        public void DestroyAmoeba(Vector3Int position)
        {
            foreach (var amoeba in amoebaCollection)
            {
                if (amoeba.Position == ConvertToVector2(position))
                {
                    try
                    {
                        amoebaCollection.Remove(amoeba);
                        amoebaTilemap.SetTile(ConvertToVector3(amoeba.Position), null);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("amoeba already extinct");
                    }
                }
            }
        }
        
        // Initiate growth of amoeba, this function is called every x seconds depending on the growSpeed
        public IEnumerator Grow()
        {
            // update the grid info
            UpdateGridInfo();
            // get allowed directions to grow
            UpdateGrowLocation();
            // select a random amoeba cell to grow
            AmoebaCell amoeba = GetRandomAmoebaToGrow();
            
            float growSpeed = GetGrowSpeed(amoeba.State);
            switch (amoeba.State)
            {
                case GrowState.Fast:
                    if (growSpeed > 0)
                    {
                        mustGrow = true;
                    }
                    else
                    {
                        mustGrow = false;
                    }
                    break;
                case GrowState.Slow:
                    if (growSpeed < 4)
                    {
                        mustGrow = true;
                    }
                    else
                    {
                        mustGrow = false;
                    }

                    growSpeed /= 4;
                    break;
            }
            // the position of the tile amoeba will grow to 
            Vector3Int direction = ConvertToVector3(amoeba.GrowDirection.First());
            
            if (mustGrow)
            {
                // create a new amoeba and add it to the list
                amoebaCollection.Add(
                    new AmoebaCell(
                        amoeba.Amoeba,
                        null,
                        amoeba.GrowDirection.First(),
                        defaultState
                    )
                );
                // delete dirt if needed
                dirtTilemap.SetTile(direction, null);
                // place that amoeba on the scene
                amoebaTilemap.SetTile(direction, amoeba.Amoeba);
                // check if the tile is set
                if (amoebaTilemap.HasTile(direction))
                {
                    mustGrow = false;
                }
               
            }
            
            //waits for growSpeed amount of seconds till next grow
            yield return new WaitForSeconds(growSpeed);
            if (!mustGrow)
            {
                isGrowing = false;
            }
        }

        // iterate through all tiles to get amoeba mother cells on start
        public void GetAmoebaCells()
        {
            for (int n = amoebaTilemap.cellBounds.xMin; n < amoebaTilemap.cellBounds.xMax; n++)
            {
                for (int p = amoebaTilemap.cellBounds.yMin; p < amoebaTilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = new Vector3Int(n, p, 0);
                    Vector2Int location = new Vector2Int(localPlace.x, localPlace.y);
                       
                    if (amoebaTilemap.HasTile(localPlace))
                    {
                        amoebaCollection.Add(
                            new AmoebaCell(
                                amoebaTilemap.GetTile(localPlace), 
                                null,
                                location,
                                defaultState)
                        ); 
                    }
                }
            }
        }

        // Get all directions amoeba is allowed to grow
        public void UpdateGrowLocation()
        {
            isDormant = false;
            // goes ++ if there is a direction to grow
            int iterator = 0;
            foreach (var amoebaCell in amoebaCollection)
            {
                List<Vector2Int> growDirection = new List<Vector2Int>();
                Vector2Int curentPos = amoebaCell.Position;
                GetAllowedNeighbourTiles(curentPos);

                if (neighbourTiles != null)
                {
                    foreach (var tile in neighbourTiles)
                    {
                        if (tile.Value == "Dirt" || tile.Value == "Void")
                        {
                            // store the available locations as Vector2
                            growDirection.Add(tile.Key);
                            // add +1 to the iterator, this means amoeba can grow 
                            iterator++;
                        }
                    }
                    
                    amoebaCell.GrowDirection.Clear();
                    amoebaCell.GrowDirection.AddRange(growDirection);
                
                    switch (amoebaCell.GrowDirection.Count)
                    {
                        case 1:
                            amoebaCell.GrowCycle = 3;
                            break;
                        case 2:
                            amoebaCell.GrowCycle = 2;
                            break;
                        case 3:
                            amoebaCell.GrowCycle = 1;
                            break;
                        case 4:
                            // 4 directions, set cycle to 0
                            amoebaCell.GrowCycle = 0;
                            break;
                        case 0:
                            // amoeba can't grow , set cycle to 4 and put it to sleep
                            amoebaCell.GrowCycle = 4;
                            amoebaCell.State = GrowState.Sleeping;
                            break;
                    }
                }
            }

            if (iterator == 0)
            {
                // if isDormant is true that means amoeba can't grow anymore
                isDormant = true;
            }
           
        }


        // get neighbour tiles for each amoeba
        public void GetAllowedNeighbourTiles(Vector2Int position)
        {
            neighbourTiles.Clear();
            
            string tileName;
            Vector2Int[] directions = 
            {
                Vector2Int.down,
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.left
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

        // determines grow speed in seconds depending on the amoeba state, a random float chosen between some predetermined values
        public float GetGrowSpeed(GrowState state)
        {
            float growingSpeed;
            
            switch (state)
            {
                case GrowState.Slow:
                    // if < 4
                    growingSpeed = (random.Next(0, 128) + random.Next(0, 128)) % 128;
                    break;
                case GrowState.Fast:
                    // if > 1
                    growingSpeed = (random.Next(0, 4) + random.Next(0, 4)) % 4;
                    break;
                default:
                    growingSpeed = 0;
                    break;
            }
            // set this to desired value for testing purposes
            return growingSpeed;
        }


        
        // select a random amoeba and a random direction
        public AmoebaCell GetRandomAmoebaToGrow()
        {
            int index;
           
            // filter sleeping cells
            IEnumerable<AmoebaCell> filteringQuery =
                from cell in amoebaCollection
                where cell.State != GrowState.Sleeping
                select cell;
            
            List<AmoebaCell> allowedToGrowCells = new List<AmoebaCell>();
            // get all directions in which amoeba can grow
            foreach (var cell in filteringQuery)
            {
                allowedToGrowCells.Add(cell);
            }
            
            AmoebaCell amoeba;
            // select a random amoeba
            index = (random.Next(allowedToGrowCells.Count) + random.Next(allowedToGrowCells.Count)) % allowedToGrowCells.Count;
            amoeba = allowedToGrowCells[index];
            int index2 = random.Next(amoeba.GrowDirection.Count);
            Vector2Int direction = amoeba.GrowDirection[index2];
            amoeba.GrowDirection.Clear();
            amoeba.GrowDirection.Add(direction);
            return amoeba;
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
    }
}