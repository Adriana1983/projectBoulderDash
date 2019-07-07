using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Helper_Scripts
{
    public class GridInfoRetriever: MonoBehaviour
    {
        public Dictionary <Vector2Int, string> GetTilemaps(Tilemap[] tilemaps, GameObject[] gameObjects)
        {
            Dictionary <Vector2Int, string> tiles = new Dictionary<Vector2Int, string>();
    
            foreach (Tilemap tilemap in tilemaps)
            {
                if (tilemap.gameObject.CompareTag("Boulder") 
                    || tilemap.gameObject.CompareTag("Diamond")
                    || tilemap.gameObject.CompareTag("CaveChange"))
                {
                    continue;
                }
                for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
                {
                    for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
                    {
                        Vector3Int localPlace = new Vector3Int(n, p, 0);
                        Vector2Int location = new Vector2Int(localPlace.x, localPlace.y);
                       
                        if (tilemap.HasTile(localPlace))
                        {
                            if (tilemap.GetTile(localPlace).name != "OriginalTileset_unity_5")
                            {
                                 tiles[location] =  tilemap.GetTile(localPlace).name.Replace("OriginalTileset_","");
                            }
                           
                        }
                    }
                }
            }
            foreach (var go in gameObjects)
            {
                Vector3 location;
                if (go == null)
                {
                    continue;
                }
                switch (go.tag)
                {
                    // get the gameobjects that are not on tilemaps
                    case "Player":
                        //get location on grid
                        location = tilemaps[0].WorldToCell(go.transform.position);
                        tiles[new Vector2Int((int)location.x, (int)location.y)] = "Player";
                        break;
                    case "Boulder":
                        //get location on grid
                        location = tilemaps[0].WorldToCell(go.transform.position);
                        tiles[new Vector2Int((int)location.x, (int)location.y)] = "Boulder";
                        break;
                    case "Diamond":
                        //get location on grid
                        location = tilemaps[0].WorldToCell(go.transform.position);
                        tiles[new Vector2Int((int)location.x, (int)location.y)] = "Diamond";
                        break;
                    case "Exitdoor":
                        //get location on grid
                        location = tilemaps[0].WorldToCell(go.transform.position);
                        tiles[new Vector2Int((int)location.x, (int)location.y)] = "Exitdoor";
                        break;
                }
            }
            return tiles;
        }

        private void Start()
        {
            
        }
    }
}

