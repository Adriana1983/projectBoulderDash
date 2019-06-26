using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Camera;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class OpeningScreen : MonoBehaviour
{
    private int selectedOption = 1;
    private AudioClip clip;
    private bool clipPlaying;
    private float clipTimer;
    private int amountOfPlayers = 1;
    private int selectedCaveNumber = 0;
    private string selectedCave;
    private int selectedCaveLevel = 1;
    private float blinkTimer;
    public List<string> caveSelectieList = new List<string>();

    void Start()
    {
        //Lijst van begin caves
        clip = SoundManager.PlayBDmusicLoop;
        caveSelectieList.InsertRange(caveSelectieList.Count, new string[] {"A", "E", "I", "M"});
    }
    
    void Update()
    {
        //Loop so the background music keeps playing
        if (clipPlaying == false)
        {
            SoundManager.Instance.PlayBDmusic();
            clipPlaying = true;
            clipTimer = clip.length;            
        }
        
        if (clipPlaying)
        {
            clipTimer -= Time.deltaTime;
            if (clipTimer < 0)
            {
                clipTimer = clip.length;
                clipPlaying = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedOption == 4)
            {
                selectedOption = 0;
            }
            selectedOption += 1;
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedOption == 1)
            {
                selectedOption = 5;
            }
            selectedOption -= 1;
        }
        
        //Selectie Players
        if (selectedOption == 1)
        {
            GameObject.Find("Players").GetComponent<Outline>().effectColor = Color.red;
            GameObject.Find("Players").transform.Find("PlayersAmount").GetComponent<Text>().color = Color.red;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (amountOfPlayers == 2)
                {
                    amountOfPlayers = 0;
                }
                amountOfPlayers += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (amountOfPlayers == 1)
                {
                    amountOfPlayers = 3;
                }
                amountOfPlayers -= 1;
            }
            GameObject.Find("Players").transform.Find("PlayersAmount").GetComponent<Text>().text = amountOfPlayers.ToString();
        }
        else
        {
            GameObject.Find("Players").GetComponent<Outline>().effectColor = Color.black;
            GameObject.Find("Players").transform.Find("PlayersAmount").GetComponent<Text>().color = Color.white;
        }
        
        //Selectie Caves
        if (selectedOption == 2)
        {
            GameObject.Find("Cave").GetComponent<Outline>().effectColor = Color.red;
            GameObject.Find("Cave").transform.Find("CaveLevel").GetComponent<Text>().color = Color.red;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedCaveNumber == caveSelectieList.Count - 1)
                {
                    selectedCaveNumber = -1;
                }

                selectedCaveNumber++;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedCaveNumber == 0)
                {
                    selectedCaveNumber = caveSelectieList.Count;
                }

                selectedCaveNumber--;
            }

            selectedCave = caveSelectieList[selectedCaveNumber];
            GameObject.Find("Cave").transform.Find("CaveLevel").GetComponent<Text>().text =
                selectedCave;
        }
        else
        {
            GameObject.Find("Cave").GetComponent<Outline>().effectColor = Color.black;
            GameObject.Find("Cave").transform.Find("CaveLevel").GetComponent<Text>().color = Color.white;
        }
        
        //SelectieLevels
        if (selectedOption == 3)
        {
            GameObject.Find("Level").GetComponent<Outline>().effectColor = Color.red;
            GameObject.Find("Level").transform.Find("SelectedLevel").GetComponent<Text>().color = Color.red;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedCaveLevel == 5)
                {
                    selectedCaveLevel = 0;
                }
                selectedCaveLevel += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedCaveLevel == 1)
                {
                    selectedCaveLevel = 6;
                }
                selectedCaveLevel -= 1;
            }
            GameObject.Find("Level").transform.Find("SelectedLevel").GetComponent<Text>().text = selectedCaveLevel.ToString();
        }
        else
        {
            GameObject.Find("Level").GetComponent<Outline>().effectColor = Color.black;
            GameObject.Find("Level").transform.Find("SelectedLevel").GetComponent<Text>().color = Color.white;

        }

        //Play button / Press enter to start
        if (Input.GetKeyDown(KeyCode.Return))
        {
                SoundManager.Instance.PlayCollectdiamond();
                SoundManager.Instance.StopAllAudio();
                //Settings for the script caveloader
                PlayerPrefs.SetString("Cave", caveSelectieList[selectedCaveNumber]);
                PlayerPrefs.SetInt("Level", selectedCaveLevel);
                
                PlayerPrefs.SetInt("Players", amountOfPlayers);
              
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //Blinking loop
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SoundManager.Instance.PlayCollectdiamond();            
        }
        
        blinkTimer += Time.deltaTime;
        if (blinkTimer < 0.5f)
        {
            GameObject.Find("Play").GetComponent<Text>().text = "";
        }

        if (blinkTimer > 0.5f)
        {
            GameObject.Find("Play").GetComponent<Text>().text = "^ Press Enter ^ " +
                                                                "To Play";
        }
        if (blinkTimer >= 1.25f)
        {
            blinkTimer = 0;
        }
    }

    
}
