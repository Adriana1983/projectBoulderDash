using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    #region Singleton
    private static Score instance;
    public static Score Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Score>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned Score Manager", typeof(Score)).GetComponent<Score>();
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    //cave variables
    public char CurrentCave = 'G';

    public float caveTime = 150;
    public float amoebaMagicTime = 75; //this is both amoebaSlowGrowthTime as magicWallMillingTime
    public int initialDiamondsValue = 50;
    public int extraDiamondsValue = 90;
    public int diamondsNeeded = 75;
    public int diamondsCollected = 0;

    public int life = 3;

    public bool Finish = false;
    private int extraLifeScore = 0;
    private int score = 0;
    public int TotalScore
    {
        get
        {
            return score;
        }
        set
        {
            extraLifeScore = value;
            if (extraLifeScore > 500)
            {
                extraLifeScore = 0;
                life++;
            }
            score = value;
        }
    }


    public void SetCaveData(List<string> caveSettings)
    {
        caveTime = float.Parse(caveSettings[0]);
        amoebaMagicTime = float.Parse(caveSettings[1]);
        initialDiamondsValue = int.Parse(caveSettings[2]);
        extraDiamondsValue = int.Parse(caveSettings[3]);
        diamondsNeeded = int.Parse(caveSettings[4]);
        diamondsCollected = 0;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
