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

    public Dictionary<int, string> cavesToNumbers = new Dictionary<int, string>();
    private int SelectedCave;
    private int SelectedLevel;

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
        return Tiles[(int) t];
    }

    private GameObject GetPrefab(Prefabs p)
    {
        return Prefab[(int) p];
    }

    void Awake()
    {
        int number = 0;
        List<string> cavelist = new List<string>();
        cavelist.InsertRange(cavelist.Count,
            new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P"});
        foreach (var caves in cavelist)
        {
            cavesToNumbers.Add(number, caves);
            number++;
        }

        SelectedCave = PlayerPrefs.GetInt("Cave");
        SelectedLevel = PlayerPrefs.GetInt("Level");

        //loading text file and separating by breaklines
        List<string> caveDataList = new List<string>();
        TextAsset caveData;
        caveData = (TextAsset) Resources.Load("Caves/Levels/Cave" + cavesToNumbers[SelectedCave] + "-" + SelectedLevel);
        caveDataList = caveData.text.Trim().Split('\n').Reverse().ToList();

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
                        var s = GameObject.Instantiate(GetPrefab(Prefabs.SpawnRockford),
                            new Vector3(x + 0.5f, y + 0.5f, 0),
                            Quaternion.identity);

                        break;

                    case 'd':
                        var d = GameObject.Instantiate(GetPrefab(Prefabs.Diamond),
                            new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);

                        break;
                    case 'm':
                        var m = GameObject.Instantiate(GetPrefab(Prefabs.MagicWall),
                            new Vector3(x + 0.5f, y + 0.5f, 0),
                            Quaternion.identity);
                        break;

                    #endregion
                }
            }
        }
    }
}

  
