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
    public Tilemap Firefly;
    public Tilemap Butterfly;

    public TileBase[] Tiles;
    public GameObject[] Prefab;

    public Text CaveAndIntermissionUI;

    int LastSound = 0;


    enum Tile
    {
        Bounds = 0,
        Dirt = 1,
        Wall = 2,
        Amoeba = 3,
        Firefly = 4,
        Butterfly = 5
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
        //TextAsset caveData = (TextAsset)Resources.Load("Caves/Levels/Testcave");
        //TextAsset caveData = (TextAsset)Resources.Load("Caves/Levels/CaveT-1");
        TextAsset caveData = (TextAsset)Resources.Load($"Caves/Levels/Cave{Score.Instance.currentCave}-{Score.Instance.currentLevel}");
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
                    case 'q':
                        Firefly.SetTile(pos, GetTile(Tile.Firefly));
                        break;
                    case 'B':
                        Butterfly.SetTile(pos, GetTile(Tile.Butterfly));
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

        SoundManager.Instance.PlayCover();
        Score.Instance.Finish = false;
    }

    private void Update()
    {
        //Animation removing tiles from ChangeCave layer
        if (ChangeCavePositionsLoad.Count > 0)
        {

            if (Score.Instance.currentCave == 'Q' || Score.Instance.currentCave == 'R' || Score.Instance.currentCave == 'S' || Score.Instance.currentCave == 'T')
            {
                CaveAndIntermissionUI.text = "    <color=white>B O N U S   L I F E</color>    ";
            }
            else
            {
                //show player who's turn it is, number of lives he has and cave + level that's loading
                CaveAndIntermissionUI.text = $"    <color=white>player 1</color>   <color=white>{Score.Instance.life} MEN</color>   <color=white>CAVE {Score.Instance.currentCave}/{Score.Instance.currentLevel}</color>";
            }


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
            if (GameObject.FindGameObjectWithTag("Player") != null && Score.Instance.Finish == false && Score.Instance.caveTime > 0)
            {
                Score.Instance.caveTime -= Time.deltaTime;
                ////the time a Timeout pitch is aloud to play and the time between two Timeout pitches listens very closely
                ////there's a high chance that at a high or low fps these Timeout pitches get played more than once or not at all
                if (Score.Instance.caveTime < 9.5 && LastSound < 1)
                {
                    LastSound = 1;
                    SoundManager.Instance.PlayTimeout1();
                }
                if (Score.Instance.caveTime < 8.5 && LastSound < 2)
                {
                    LastSound = 2;
                    SoundManager.Instance.PlayTimeout2();
                }
                if (Score.Instance.caveTime < 7.5 && LastSound < 3)
                {
                    LastSound = 3;
                    SoundManager.Instance.PlayTimeout3();
                }
                if (Score.Instance.caveTime < 6.5 && LastSound < 4)
                {
                    LastSound = 4;
                    SoundManager.Instance.PlayTimeout4();
                }
                if (Score.Instance.caveTime < 5.5 && LastSound < 5)
                {
                    LastSound = 5;
                    SoundManager.Instance.PlayTimeout5();
                }
                if (Score.Instance.caveTime < 4.5 && LastSound < 6)
                {
                    LastSound = 6;
                    SoundManager.Instance.PlayTimeout6();
                }
                if (Score.Instance.caveTime < 3.5 && LastSound < 7)
                {
                    LastSound = 7;
                    SoundManager.Instance.PlayTimeout7();
                }
                if (Score.Instance.caveTime < 2.5 && LastSound < 8)
                {
                    LastSound = 8;
                    SoundManager.Instance.PlayTimeout8();
                }
                if (Score.Instance.caveTime < 1.5 && LastSound < 9)
                {
                    LastSound = 9;
                    SoundManager.Instance.PlayTimeout9();
                }
            }
            if (Score.Instance.diamondsCollected < Score.Instance.diamondsNeeded)
            {
                CaveAndIntermissionUI.text = $"<color=yellow>{Score.Instance.diamondsNeeded}</color> <color=white>\\</color> <color=white>{Score.Instance.initialDiamondsValue}</color>   <color=yellow>{Score.Instance.diamondsCollected}</color>   <color=white>{Score.Instance.caveTime.ToString("000")}</color>    <color=white>{Score.Instance.TotalScore.ToString("D6")}</color>";
            }
            else
            {
                CaveAndIntermissionUI.text = $"<color=yellow>{Score.Instance.diamondsNeeded}</color> <color=white>\\</color> <color=white>{Score.Instance.extraDiamondsValue}</color>   <color=yellow>{Score.Instance.diamondsCollected}</color>   <color=white>{Score.Instance.caveTime.ToString("000")}</color>    <color=white>{Score.Instance.TotalScore.ToString("D6")}</color>";
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
}
