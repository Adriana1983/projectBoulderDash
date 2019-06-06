using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource bdmusic; //opening tune
    AudioSource cover; //building up cave/intermission
    AudioSource crack; //Rockford appears in cave - he's immediately able to move - cave timer starts running -- opening exit
    AudioSource finished; //upon completing cave/intermission remaining seconds turn in to score

    AudioSource amoeba; //when amoeba grows
    AudioSource explosion; //all explosions
    AudioSource boulder; //when a boulder falls on dirt/boulder/diamond/amoeba/wall/titaninum wall (NOT on magic wall)
    AudioSource box_push; //pushing boulder
    AudioSource walk_empty;
    AudioSource walk_dirt; 
    
    AudioSource collectdiamond;
    AudioSource diamond1; //when a diamond falls on dirt/boulder/diamond/amoeba/wall/titaninum wall (NOT on magic wall)
    AudioSource diamond2; //when more then 1 diamond fell on dirt/boulder/diamond/amoeba/Wall/titaninum wall
    AudioSource diamond3; //in a very short time period each fallen diamond has it's on sound-pitch
    AudioSource diamond4; //when more then 6 diamonds have fallen the cycle of sound-pitches starts over 
    AudioSource diamond5; //and the 7th diamond gets the sound-pitch of the 1st diamond
    AudioSource diamond6;
    AudioSource magic_wall;

    AudioSource timeout1; //9 sec remaining befor "out of time"
    AudioSource timeout2; //8 sec remaining befor "out of time"
    AudioSource timeout3; //7 sec remaining befor "out of time"
    AudioSource timeout4; //6 sec remaining befor "out of time"
    AudioSource timeout5; //5 sec remaining befor "out of time"
    AudioSource timeout6; //4 sec remaining befor "out of time"
    AudioSource timeout7; //3 sec remaining befor "out of time"
    AudioSource timeout8; //2 sec remaining befor "out of time"
    AudioSource timeout9; //1 sec remaining befor "out of time"



    // Start is called before the first frame update
    void Start()
    {
        bdmusic = GameObject.Find("bdmusic").GetComponent<AudioSource>();
        cover = GameObject.Find("cover").GetComponent<AudioSource>();
        crack = GameObject.Find("crack").GetComponent<AudioSource>();
        finished = GameObject.Find("finished").GetComponent<AudioSource>();

        amoeba = GameObject.Find("amoeba").GetComponent<AudioSource>();
        explosion = GameObject.Find("explosion").GetComponent<AudioSource>();
        boulder = GameObject.Find("boulder").GetComponent<AudioSource>();
        box_push = GameObject.Find("box_push").GetComponent<AudioSource>();
        walk_empty = GameObject.Find("walk_empty").GetComponent<AudioSource>();
        walk_dirt = GameObject.Find("walk_dirt").GetComponent<AudioSource>();

        collectdiamond = GameObject.Find("collectdiamond").GetComponent<AudioSource>();
        diamond1 = GameObject.Find("diamond1").GetComponent<AudioSource>();
        diamond2 = GameObject.Find("diamond2").GetComponent<AudioSource>();
        diamond3 = GameObject.Find("diamond3").GetComponent<AudioSource>();
        diamond4 = GameObject.Find("diamond4").GetComponent<AudioSource>();
        diamond5 = GameObject.Find("diamond5").GetComponent<AudioSource>();
        diamond6 = GameObject.Find("diamond6").GetComponent<AudioSource>();
        magic_wall = GameObject.Find("magic_wall").GetComponent<AudioSource>();

        timeout1 = GameObject.Find("timeout1").GetComponent<AudioSource>();
        timeout2 = GameObject.Find("timeout2").GetComponent<AudioSource>();
        timeout3 = GameObject.Find("timeout3").GetComponent<AudioSource>();
        timeout4 = GameObject.Find("timeout4").GetComponent<AudioSource>();
        timeout5 = GameObject.Find("timeout5").GetComponent<AudioSource>();
        timeout6 = GameObject.Find("timeout6").GetComponent<AudioSource>();
        timeout7 = GameObject.Find("timeout7").GetComponent<AudioSource>();
        timeout8 = GameObject.Find("timeout8").GetComponent<AudioSource>();
        timeout9 = GameObject.Find("timeout9").GetComponent<AudioSource>();
    }
    
    public void PlayBDmusic()
    {
        bdmusic.Play();
    }

    public void PlayCover()
    {
        cover.Play();
    }

    public void PlayCrack()
    {
        crack.Play();
    }

    public void PlayFinished()
    {
        finished.Play();
    }

    public void PlayAmoeba()
    {
        amoeba.Play();
    }

    public void PlayExplosion()
    {
        explosion.Play();
    }

    public void PlayBoulder()
    {
        boulder.Play();
    }

    public void PlayBox_push()
    {
        box_push.Play();
    }

    public void PlayWalkEmpty()
    {
        walk_empty.Play();  
    }

    public void PlayWalkDirt()
    {
        walk_dirt.Play();
    }

    public void PlayCollectdiamond()
    {
        collectdiamond.Play();
    }

    public void PlayDiamond1()
    {
        diamond1.Play();
    }

    public void PlayDiamond2()
    {
        diamond2.Play();
    }

    public void PlayDiamond3()
    {
        diamond3.Play();
    }

    public void PlayDiamond4()
    {
        diamond4.Play();
    }

    public void PlayDiamond5()
    {
        diamond5.Play();
    }

    public void PlayDiamond6()
    {
        diamond6.Play();
    }

    public void PlayMagicWall()
    {
        magic_wall.Play();
    }

    public void PlayTimeout1()
    {
        timeout1.Play();
    }

    public void PlayTimeout2()
    {
        timeout2.Play();
    }

    public void PlayTimeout3()
    {
        timeout3.Play();
    }

    public void PlayTimeout4()
    {
        timeout4.Play();
    }

    public void PlayTimeout5()
    {
        timeout5.Play();
    }

    public void PlayTimeout6()
    {
        timeout6.Play();
    }

    public void PlayTimeout7()
    {
        timeout7.Play();
    }

    public void PlayTimeout8()
    {
        timeout8.Play();
    }

    public void PlayTimeout9()
    {
        timeout9.Play();
    }
}
