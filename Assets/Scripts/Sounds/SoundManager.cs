using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=tLyj02T51Oc
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned SoundManager", typeof(SoundManager)).GetComponent<SoundManager>();
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private AudioSource sfxSource;

    public AudioClip bdmusic; //opening tune
    public AudioClip cover; //building up cave/intermission
    public AudioClip crack; //Rockford appears in cave - he's immediately able to move - cave timer starts running -- opening exit
    public AudioClip finished; //upon completing cave/intermission remaining seconds turn in to score

    public AudioClip amoeba; //when amoeba grows
    public AudioClip explosion; //all explosions
    public AudioClip boulder; //when a boulder falls on dirt/boulder/diamond/amoeba/wall/titaninum wall (NOT on magic wall)
    public AudioClip box_push; //pushing boulder
    public AudioClip walk_empty;
    public AudioClip walk_dirt;

    public AudioClip collectdiamond;
    public AudioClip diamond1; //when a diamond falls on dirt/boulder/diamond/amoeba/wall/titaninum wall (NOT on magic wall)
    public AudioClip diamond2; //when more then 1 diamond fell on dirt/boulder/diamond/amoeba/Wall/titaninum wall
    public AudioClip diamond3; //in a very short time period each fallen diamond has it's on sound-pitch
    public AudioClip diamond4; //when more then 6 diamonds have fallen the cycle of sound-pitches starts over 
    public AudioClip diamond5; //and the 7th diamond gets the sound-pitch of the 1st diamond
    public AudioClip diamond6;
    public AudioClip magic_wall;

    public AudioClip timeout1; //9 sec remaining befor "out of time"
    public AudioClip timeout2; //8 sec remaining befor "out of time"
    public AudioClip timeout3; //7 sec remaining befor "out of time"
    public AudioClip timeout4; //6 sec remaining befor "out of time"
    public AudioClip timeout5; //5 sec remaining befor "out of time"
    public AudioClip timeout6; //4 sec remaining befor "out of time"
    public AudioClip timeout7; //3 sec remaining befor "out of time"
    public AudioClip timeout8; //2 sec remaining befor "out of time"
    public AudioClip timeout9; //1 sec remaining befor "out of time"



    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        sfxSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
        //else
            //Debug.Log("Audio clip is null");
    }

    public void PlayBDmusic()
    {
        PlaySound(bdmusic);
    }

    public void PlayCover()
    {
        PlaySound(cover);
    }

    public void PlayCrack()
    {
        PlaySound(crack);
    }

    public void PlayFinished()
    {
        PlaySound(finished);
    }

    public void PlayAmoeba()
    {
        PlaySound(amoeba);
    }

    public void PlayExplosion()
    {
        PlaySound(explosion);
    }

    public void PlayBoulder()
    {
        PlaySound(boulder);
    }

    public void PlayBox_push()
    {
        PlaySound(box_push);
    }

    public void PlayWalkEmpty()
    {
        PlaySound(walk_empty);
    }

    public void PlayWalkDirt()
    {
        PlaySound(walk_dirt);
    }

    public void PlayCollectdiamond()
    {
        PlaySound(collectdiamond);
    }

    public void PlayDiamond1()
    {
        PlaySound(diamond1);
    }

    public void PlayDiamond2()
    {
        PlaySound(diamond2);
    }

    public void PlayDiamond3()
    {
        PlaySound(diamond3);
    }

    public void PlayDiamond4()
    {
        PlaySound(diamond4);
    }

    public void PlayDiamond5()
    {
        PlaySound(diamond5);
    }

    public void PlayDiamond6()
    {
        PlaySound(diamond6);
    }

    public void PlayMagicWall()
    {
        PlaySound(magic_wall);
    }

    public void PlayTimeout1()
    {
        PlaySound(timeout1);
    }

    public void PlayTimeout2()
    {
        PlaySound(timeout2);
    }

    public void PlayTimeout3()
    {
        PlaySound(timeout3);
    }

    public void PlayTimeout4()
    {
        PlaySound(timeout4);
    }

    public void PlayTimeout5()
    {
        PlaySound(timeout5);
    }

    public void PlayTimeout6()
    {
        PlaySound(timeout6);
    }

    public void PlayTimeout7()
    {
        PlaySound(timeout7);
    }

    public void PlayTimeout8()
    {
        PlaySound(timeout8);
    }

    public void PlayTimeout9()
    {
        PlaySound(timeout9);
    }
}
