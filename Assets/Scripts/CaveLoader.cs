using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveLoader : MonoBehaviour
{
    private int Width, Height = 0;

    public Tilemap Bounds;
    public Tilemap Dirt;
    public Tilemap Boulders;
    public Tilemap Wall;

    public TileBase[] Tiles;
    public GameObject[] Prefab;

    enum Tile
    {
        Bounds = 0,
        Dirt = 1,
        Wall = 2
    }

    enum Prefabs
    {
        Boulder = 0,
        SpawnRockford = 1,
        Diamond = 2,
        MagicWall = 3
    }


    private TileBase GetTile(Tile t)
    {
        return Tiles[(int)t];
    }

    private GameObject GetPrefab(Prefabs p)
    {
        return Prefab[(int)p];
    }

    void Awake()
    {
        //loading text file and separating by breaklines
        string SelectedCave = PlayerPrefs.GetString("Cave");
        int SelectedLevel = PlayerPrefs.GetInt("Level");
        if (SelectedCave != null)
        {
            TextAsset caveData;
            if (SelectedCave.Length == 1)
            {
                caveData = (TextAsset) Resources.Load("Caves/Levels/Cave" + SelectedCave + "-" + SelectedLevel);
            }
            else
            {
                caveData = (TextAsset) Resources.Load("Caves/Levels/" + SelectedCave);
            }
            List<string> caveDataList = caveData.text.Trim().Split('\n').Reverse().ToList();

            Height = caveDataList.Count;
            Width = caveDataList[0].Length;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    switch (caveDataList[y][x])
                    {
                        #region Tiles

                        case 'W':
                            Bounds.SetTile(new Vector3Int(x, y, 0), GetTile(Tile.Bounds));
                            break;
                        case 'w':
                            Wall.SetTile(new Vector3Int(x, y, 0), GetTile(Tile.Wall));
                            break;

                        case '.':
                            Dirt.SetTile(new Vector3Int(x, y, 0), GetTile(Tile.Dirt));
                            break;

                        #endregion

                        #region Prefabs

                        case 'r':
                            var b = GameObject.Instantiate(GetPrefab(Prefabs.Boulder),
                                new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                            b.transform.parent = Boulders.transform;
                            break;

                        case 'X':
                            GameObject.Instantiate(GetPrefab(Prefabs.SpawnRockford), new Vector3(x + 0.5f, y + 0.5f, 0),
                                Quaternion.identity);
                            break;

                        case 'd':
                            GameObject.Instantiate(GetPrefab(Prefabs.Diamond), new Vector3(x + 0.5f, y + 0.5f, 0),
                                Quaternion.identity);
                            break;
                        case 'm':
                            GameObject.Instantiate(GetPrefab(Prefabs.MagicWall), new Vector3(x + 0.5f, y + 0.5f, 0),
                                Quaternion.identity);
                            break;

                        #endregion
                    }
                }
            }
        }
    }
}
