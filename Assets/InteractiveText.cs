using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveText : MonoBehaviour
{
    void OnMouseEnter()
    {
       gameObject.GetComponent<Text>().color = Color.red;  
    }
    void Start()
    {

    }
}
