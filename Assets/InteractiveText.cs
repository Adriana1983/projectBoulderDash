using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class InteractiveText : MonoBehaviour
{
    private int selectedOption = 1;
    private int amountOfPlayers = 1;
    private int selectedCaveNumber = 1;
    private string selectedCave = "A";
    private int selectedCaveLevel = 1;
    public List<string> cavelist = new List<string>();
    public 
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedOption == 3)
            {
                selectedOption = 0;
            }
            selectedOption += 1;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedOption == 1)
            {
                selectedOption = 4;
            }
            selectedOption -= 1;
        }
        if (selectedOption == 1)
        {
            gameObject.transform.Find("Players").GetComponent<Text>().color = Color.gray;
            GameObject.Find("Button").transform.Find("PlayersAmount").GetComponent<Text>().color = Color.red;

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
            GameObject.Find("Button").transform.Find("PlayersAmount").GetComponent<Text>().text = amountOfPlayers.ToString();
        }
        else
        {
            gameObject.transform.Find("Players").GetComponent<Text>().color = Color.white;
            GameObject.Find("Button").transform.Find("PlayersAmount").GetComponent<Text>().color = Color.white;
        }
        
        if (selectedOption == 2)
        {
            /*
            gameObject.transform.Find("Cave").GetComponent<Text>().color = Color.gray;
            GameObject.Find("Button").transform.Find("Cave").GetComponent<Text>().color = Color.red;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedCaveNumber == 3)
                {
                    selectedCaveNumber = 0;
                }
                selectedCaveNumber += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedCaveNumber == 1)
                {
                    selectedCaveNumber = 4;
                }
                selectedCaveNumber -= 1;
            }

            if (selectedCaveNumber == 1)
            {
                selectedCave = "A";
            }
            if (selectedCaveNumber == 2)
            {
                selectedCave = "B";
            }
            if (selectedCaveNumber == 3)
            {
                selectedCave = "C";
            }
            GameObject.Find("Button").transform.Find("Cave").GetComponent<Text>().text = selectedCave;*/
            
            //cavelist.Add();
        }
        else
        {
            gameObject.transform.Find("Cave").GetComponent<Text>().color = Color.white;
            GameObject.Find("Button").transform.Find("Cave").GetComponent<Text>().color = Color.white;
        }

        if (selectedOption == 3)
        {
            gameObject.transform.Find("Level").GetComponent<Text>().color = Color.gray;
            GameObject.Find("Button").transform.Find("CaveLevel").GetComponent<Text>().color = Color.red;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectedCaveLevel == 2)
                {
                    selectedCaveLevel = 0;
                }
                selectedCaveLevel += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectedCaveLevel == 1)
                {
                    selectedCaveLevel = 3;
                }
                selectedCaveLevel -= 1;
            }
            GameObject.Find("Button").transform.Find("CaveLevel").GetComponent<Text>().text = selectedCaveLevel.ToString();
        }
        else
        {
            gameObject.transform.Find("Level").GetComponent<Text>().color = Color.white;
            GameObject.Find("Button").transform.Find("CaveLevel").GetComponent<Text>().color = Color.white;

        }
    }
}
