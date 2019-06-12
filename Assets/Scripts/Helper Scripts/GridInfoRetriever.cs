using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Helper_Scripts
{
    public class GridInfoRetriever: MonoBehaviour
    {
        public BoundsInt bounds;
        public TileBase[] allTiles;
        
        public Dictionary <Vector2Int, string> GetTilemaps(Tilemap[] tilemaps)
        {
           
            Dictionary <Vector2Int, string> tiles = new Dictionary<Vector2Int, string>();
            
            foreach (Tilemap tilemap in tilemaps)
            {
                bounds = tilemap.cellBounds;
                allTiles = tilemap.GetTilesBlock(bounds);
                
                for (int x = 0; x < bounds.size.x; x++) {
                    for (int y = 0; y < bounds.size.y; y++) {
                        TileBase tile = allTiles[x + y * bounds.size.x];
                        tiles[new Vector2Int(x, y - 3)] =  "Void";
                        if (tile != null)
                        {
                            // y - 3, cause it offsets the y for some reason
                            tiles[new Vector2Int(x, y - 3)] =  tile.name;
                        }
                    }
                }
            }
            return tiles;
        }

        private void Start()
        {
            
        }
    }
}

