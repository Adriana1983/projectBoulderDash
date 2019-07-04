using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

public class CaveLoader : MonoBehaviour
{
    Random ran = new Random();
    private int Width, Height = 0;

    public Tilemap Bounds;
    public Tilemap Dirt;
    public Tilemap Boulders;
    public Tilemap Wall;
    public Tilemap Amoeba;
    public Tilemap Diamond;
    public Tilemap ChangeCave;

    public TileBase[] Tiles;
    public GameObject[] Prefab;

    public Text CaveAndIntermissionUI;
    public Color randomcolor;

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

    List<Vector3Int> ChangeCavePositionsLoad = new List<Vector3Int>();
    List<Vector3Int> ChangeCavePositionsUnload = new List<Vector3Int>();

    void Awake()
    {       
        Time.timeScale = 0f;


        //loading text file and separating by breaklines
        /*TextAsset caveData = (TextAsset)Resources.Load("Caves/Levels/Testcave");*/
        //TextAsset caveData = (TextAsset)Resources.Load("Caves/Levels/CaveT-1");
        TextAsset caveData = (TextAsset)Resources.Load($"Caves/Levels/Cave{Score.Instance.CurrentCave}-1");
        List<string> caveDataList = caveData.text.Trim().Split('\n').Reverse().ToList();
        List<string> caveSettings = caveDataList.Last().Split(',').ToList();

        Score.Instance.SetCaveData(caveSettings);

        caveDataList.RemoveAt(caveDataList.Count() - 1);

        Height = caveDataList.Count;
        Width = caveDataList[0].Length;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                //New tile position
                var pos = new Vector3Int(x, y, 0);
                //Remember position
                ChangeCavePositionsLoad.Add(pos);
                ChangeCavePositionsUnload.Add(pos);
                //Add bounds tile (change cave animation)
                ChangeCave.SetTile(pos, GetTile(Tile.Bounds));

                switch (caveDataList[y][x])
                {
                    #region Tiles
                    case 'W':
                        Bounds.SetTile(pos, GetTile(Tile.Bounds));
                        break;
                    case 'w':
                        Wall.SetTile(pos, GetTile(Tile.Wall));
                        break;
                    case '.':
                        Dirt.SetTile(pos, GetTile(Tile.Dirt));
                        break;
                    case 'a':
                        Amoeba.SetTile(pos, GetTile(Tile.Amoeba));
                        break;
                    #endregion

                    #region Prefabs
                    case 'r':
                        Instantiate(GetPrefab(Prefabs.Boulder), pos, Quaternion.identity, Boulders.transform);
                        break;
                    case 'X':
                        GameObject.Instantiate(GetPrefab(Prefabs.SpawnRockford), pos, Quaternion.identity);
                        break;
                    case 'd':
                        Instantiate(GetPrefab(Prefabs.Diamond), pos, Quaternion.identity, Diamond.transform);
                        break;
                    case 'm':
                        GameObject.Instantiate(GetPrefab(Prefabs.MagicWall), pos, Quaternion.identity);
                        break;
                    case 'P':
                        GameObject.Instantiate(GetPrefab(Prefabs.Exitdoor), pos, Quaternion.identity);
                        break;

                        #endregion
                }
            }
        }
        randomcolor = UnityEngine.Random.ColorHSV(0f, 1f, 0.85f, 0.9f, 1f, 1f, 1f, 1f);
        GameObject.Find("Bounds").GetComponent<Tilemap>().color = randomcolor;        
        GameObject.Find("CaveChange").GetComponent<Tilemap>().color = randomcolor;        
        //GameObject.Find("Exitdoor").GetComponent<SpriteRenderer>().color = randomcolor;
        SoundManager.Instance.PlayCover();
        Score.Instance.Finish = false;
    }

    private void Update()
    {
        //Animation removing tiles from ChangeCave layer
        if (ChangeCavePositionsLoad.Count > 0)
        {
            //toon hier de player die speelt, de levens die hij heeft en de cave/level die er geladen word
            
            //Remove 14 tiles per frame
            for (int i = 0; i < 14; i++)
            {
                if (ChangeCavePositionsLoad.Count > 0)
                {
                    //Choose random position
                    var pos = ChangeCavePositionsLoad[ran.Next(0, ChangeCavePositionsLoad.Count - 1)];
                    //Rmove tile
                    ChangeCave.SetTile(pos, null);
                    //Remove position from the list
                    ChangeCavePositionsLoad.Remove(pos);
                }
            }
        }
        else
        {
            Time.timeScale = 1;
            if (Score.Instance.Finish == false)
                Score.Instance.caveTime -= Time.deltaTime;
            if (Score.Instance.diamondsCollected < Score.Instance.diamondsNeeded)
            {
                CaveAndIntermissionUI.text = $"DR:<color=yellow>{Score.Instance.diamondsNeeded}</color> <color=white>\\</color> DV:<color=white>{Score.Instance.initialDiamondsValue}</color> DC:<color=yellow>{Score.Instance.diamondsCollected}</color> Time:<color=white>{Score.Instance.caveTime.ToString("F0")}</color> Score:<color=white>{Score.Instance.TotalScore}</color>";

            }
            else
            {
                CaveAndIntermissionUI.text = $"DR:<color=yellow>{Score.Instance.diamondsNeeded}</color> <color=white>\\</color> DV:<color=white>{Score.Instance.extraDiamondsValue}</color> DC:<color=yellow>{Score.Instance.diamondsCollected}</color> Time:<color=white>{Score.Instance.caveTime.ToString("F0")}</color> Score:<color=white>{Score.Instance.TotalScore}</color>";
            }
        }
    }

    public int FillScreen()
    {
        //Add change cave animation tiles
        for (int i = 0; i < 14; i++)
        {
            if (ChangeCavePositionsUnload.Count > 0)
            {
                var pos = ChangeCavePositionsUnload[ran.Next(0, ChangeCavePositionsUnload.Count - 1)];
                ChangeCave.SetTile(pos, GetTile(Tile.Bounds));
                ChangeCavePositionsUnload.Remove(pos);
            }
        }

        return ChangeCavePositionsUnload.Count;
    }

    private void Start()
    {       
        GameObject.Find("Bounds").GetComponent<Tilemap>().color = randomcolor;        
        GameObject.Find("RandomLight").GetComponent<Light>().color = randomcolor;
        GameObject.FindGameObjectWithTag("Exitdoor").GetComponent<SpriteRenderer>().color = randomcolor;
        GameObject.FindGameObjectWithTag("Door").GetComponent<SpriteRenderer>().color = randomcolor;
    }
}
