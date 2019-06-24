using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CaveLoader : MonoBehaviour
{
    private int Width, Height = 0;

    public Tilemap Bounds;
    public Tilemap Dirt;
    public Tilemap Boulders;
    public Tilemap Wall;
    public Tilemap Amoeba;
    public Tilemap Diamond;

    public TileBase[] Tiles;
    public GameObject[] Prefab;

    public Text CaveAndIntermissionUI;


    enum Tile
    {
        Bounds = 0,
        Dirt = 1,
        Wall = 2,
        Amoeba = 3
    }

    enum Prefabs
    {
        Boulder = 0,
        SpawnRockford = 1,
        Diamond = 2,
        MagicWall = 3,
        Exitdoor = 4
     
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
        //Time.timeScale = 0.2f;

        //loading text file and separating by breaklines
        /*TextAsset caveData = (TextAsset)Resources.Load("Caves/Levels/Testcave");*/
        TextAsset caveData = (TextAsset)Resources.Load($"Caves/Levels/Cave{Score.Instance.CurrentCave}-1");
        List<string> caveDataList = caveData.text.Trim().Split('\n').Reverse().ToList();
        List<string> caveSettings = caveDataList.Last().Split(',').ToList();

        Score.Instance.SetCaveData(caveSettings);

        caveDataList.RemoveAt(caveDataList.Count()-1);

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
                    case 'a':
                        Amoeba.SetTile(new Vector3Int(x, y, 0), GetTile(Tile.Amoeba));
                        break;
                    #endregion

                    #region Prefabs
                    case 'r':
                        Instantiate(GetPrefab(Prefabs.Boulder), new Vector3(x, y, 0), Quaternion.identity, Boulders.transform);                       
                        break;
                    case 'X':
                        GameObject.Instantiate(GetPrefab(Prefabs.SpawnRockford), new Vector3(x, y, 0), Quaternion.identity);
                        break;
                    case 'd':
                        Instantiate(GetPrefab(Prefabs.Diamond), new Vector3(x, y, 0), Quaternion.identity, Diamond.transform);
                        break;
                    case 'm':
                        GameObject.Instantiate(GetPrefab(Prefabs.MagicWall), new Vector3(x, y, 0), Quaternion.identity);
                        break;
                    case 'P':
                        GameObject.Instantiate(GetPrefab(Prefabs.Exitdoor), new Vector3(x, y, 0), Quaternion.identity);
                        break;
                    
                        #endregion
                }
            }
        }
    }

    private void Update()
    {
        Score.Instance.caveTime -= Time.deltaTime;
        CaveAndIntermissionUI.text = $"DR: {Score.Instance.diamondsNeeded.ToString("F0")} DV: {Score.Instance.initialDiamondsValue.ToString("F0")} DC: {Score.Instance.diamondsCollected}   Time: {Score.Instance.caveTime.ToString("F0")} Score: {Score.Instance.score}";
    }

}
