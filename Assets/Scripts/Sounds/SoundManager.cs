using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    AudioSource walk_empty;
    AudioSource walk_dirt;

    // Start is called before the first frame update
    void Start()
    {
        walk_empty = GameObject.Find("walk_empty").GetComponent<AudioSource>();
        walk_dirt = GameObject.Find("walk_dirt").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Play_Walk_empty()
    {
        walk_empty.Play();  
    }

    public void Play_Walk_Dirt()
    {
        walk_dirt.Play();
    }
}
