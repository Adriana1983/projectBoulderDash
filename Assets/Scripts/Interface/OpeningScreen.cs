using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
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
    private bool delayTrue;
    private float delay;
    public List<string> caveSelectieList = new List<string>();

    void Start()
    {
        //Lijst van begin caves
        clip = SoundManager.PlayBDmusicLoop;
        caveSelectieList.InsertRange(caveSelectieList.Count, new string[] {"A", "E", "I", "M"});
    }
    
    public static string GoDirection()
    {
        string direction = null;

        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetAxisRaw("PadHorizontal") == -1 ||
            Input.GetAxisRaw("LeftJoystickHorizontal") == -1)
        {
            direction = "Left";
        }

        if (direction == null)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetAxisRaw("PadHorizontal") == 1 ||
                Input.GetAxisRaw("LeftJoystickHorizontal") == 1)
            {
                direction = "Right"; 
            }
        }

        if (direction == null)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetAxisRaw("PadVertical") == -1 ||
                Input.GetAxisRaw("LeftJoystickVertical") == -1)
            {
                direction = "Down";
            }
        }
        if (direction == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || 
                Input.GetAxisRaw("PadVertical") == 1  ||
                Input.GetAxisRaw("LeftJoystickVertical") == 1)
            {
                direction = "Up";
            }
        }
    
    return direction;
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

        if (delayTrue)
        {
            delay += Time.deltaTime;
        }

        if (delay > 0.2)
        {
            delay = 0;
            delayTrue = false;
        }

        if (delayTrue == false)
        {
            if (GoDirection() == "Down")
            {
                if (selectedOption == 3)
                {
                    selectedOption = 0;
                }

                selectedOption += 1;
                delayTrue = true;
            }

            if (GoDirection() == "Up")
            {
                if (selectedOption == 1)
                {
                    selectedOption = 4;
                }

                selectedOption -= 1;
                delayTrue = true;
            }

            //Selectie Players
            if (selectedOption == 1)
            {
                GameObject.Find("Players").GetComponent<Outline>().effectColor = Color.red;
                GameObject.Find("Players").transform.Find("PlayersAmount").GetComponent<Text>().color = Color.red;

                if (GoDirection() == "Right")
                {
                    if (amountOfPlayers == 2)
                    {
                        amountOfPlayers = 0;
                    }

                    amountOfPlayers += 1;
                    delayTrue = true;
                }

                if (GoDirection() == "Left")
                {
                    if (amountOfPlayers == 1)
                    {
                        amountOfPlayers = 3;
                    }

                    amountOfPlayers -= 1;
                    delayTrue = true;
                }

                GameObject.Find("Players").transform.Find("PlayersAmount").GetComponent<Text>().text =
                    amountOfPlayers.ToString();
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
                if (GoDirection() == "Right")
                {
                    if (selectedCaveNumber == caveSelectieList.Count - 1)
                    {
                        selectedCaveNumber = -1;
                    }

                    selectedCaveNumber++;
                    delayTrue = true;
                }

                if (GoDirection() == "Left")
                {
                    if (selectedCaveNumber == 0)
                    {
                        selectedCaveNumber = caveSelectieList.Count;
                    }

                    selectedCaveNumber--;
                    delayTrue = true;
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

                if (GoDirection() == "Right")
                {
                    if (selectedCaveLevel == 5)
                    {
                        selectedCaveLevel = 0;
                    }

                    selectedCaveLevel += 1;
                    delayTrue = true;
                }

                if (GoDirection() == "Left")
                {
                    if (selectedCaveLevel == 1)
                    {
                        selectedCaveLevel = 6;
                    }

                    selectedCaveLevel -= 1;
                    delayTrue = true;
                }

                GameObject.Find("Level").transform.Find("SelectedLevel").GetComponent<Text>().text =
                    selectedCaveLevel.ToString();
            }
            else
            {
                GameObject.Find("Level").GetComponent<Outline>().effectColor = Color.black;
                GameObject.Find("Level").transform.Find("SelectedLevel").GetComponent<Text>().color = Color.white;
            }
        }
        
        //Play button / Press enter to start
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
                SoundManager.Instance.PlayCollectdiamond();
                SoundManager.Instance.StopAllAudio();
                //Settings for the script caveloader
                Debug.Log(selectedCaveLevel.ToString().ToCharArray()[0]);
                PlayerPrefs.SetString("Cave", caveSelectieList[selectedCaveNumber]);
                PlayerPrefs.SetInt("Level", selectedCaveLevel);
                
                PlayerPrefs.SetInt("Players", amountOfPlayers);
              
                SceneManager.LoadScene("SceneLoader");
        } 
        //Plays sound whenever the selection changes
        if (GoDirection() != null)
        {
            SoundManager.Instance.PlayCollectdiamond();            
        }

        //Blinking loop       
        blinkTimer += Time.deltaTime;
        if (blinkTimer < 0.5f)
        {
            GameObject.Find("Play").GetComponent<Text>().text = "";
        }

        if (blinkTimer > 0.5f)
        {
            if (Input.GetJoystickNames().Length != 0)
            {
                if (Input.GetJoystickNames()[0] != "")
                {
                    GameObject.Find("Play").GetComponent<Text>().text = "^ Press A ^     " +
                                                                        "To Play";
                }
                
            }
            else
            {
                GameObject.Find("Play").GetComponent<Text>().text = "^ Press Enter ^ " +
                                                                    "To Play";
            }
        } 

        if (blinkTimer >= 1.25f)
        {
            blinkTimer = 0;
        }
    }
}
