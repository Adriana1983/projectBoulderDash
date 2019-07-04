using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Objects;
using UnityEngine;

public class MagicWall : MonoBehaviour
{
    public Sprite wallSprite;
    public Sprite magicWallSprite;
    public GameObject Boulder;
    public GameObject Diamond;
    public bool activated;
    private float activeDuration;
    private float timer = 0.1875f;
    private SpriteRenderer wallRenderer;
    public bool firstactivated;
    private bool clipPlaying;
    private float clipTimer;
    
    private void Update()
    {

        //Als er een boulder een Magic wall heeft geactiveerd doe dit
        if (firstactivated && activeDuration == 0)
        {
            //Zoekt alle andere magic walls en zet hun variable activated naar true
            if (clipPlaying == false && firstactivated)
            {
                foreach (var variable in GameObject.FindGameObjectsWithTag("MagicWall"))
                {
                    //variable.GetComponent<SpriteRenderer>().sprite = magicWallSprite;
                    variable.GetComponent<Animator>().enabled = true;
                    variable.GetComponent<MagicWall>().activeDuration = Score.Instance.amoebaMagicTime;
                }
                firstactivated = false;
                activated = true;
            }
        }

        //if duration is still active
        if (activeDuration > 0)
        {
            if (activated)
            {
                
                AudioClip clip = SoundManager.PlayMagicWallLoop;
                if (clipPlaying == false)
                {
                    SoundManager.Instance.PlayMagicWall();
                    clipPlaying = true;
                    clipTimer = clip.length;
                }

                if (clipPlaying)
                {
                    clipTimer -= Time.deltaTime;
                    if (clipTimer < 0.1f)
                    {
                        clipTimer = clip.length;
                        clipPlaying = false;
                    }
                }
            }
            activeDuration -= Time.deltaTime;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, 1);
                RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1f);
                
                //Check of er een boulder of diamond boven de wall is en of er ruimte onder de wall zit
                if (hitup.collider != null && hitdown.collider == null)
                {
                    switch (hitup.collider.tag)
                    {
                        case "Diamond":
                            //activated = true;
                            if (hitup.collider.gameObject.GetComponent<Diamond>().lastpositionFalling)
                            {
                                Destroy(hitup.collider.gameObject);
                                Instantiate(Boulder,
                                    new Vector2(gameObject.transform.position.x,
                                        gameObject.transform.position.y - 1),
                                    Quaternion.identity);
                            }

                            break;
                        case "Boulder":
                            //activated = true;
                            if (hitup.collider.gameObject.GetComponent<Boulder>().lastpositionFalling || hitup.collider.gameObject.GetComponent<Boulder>().falling)
                            {
                                Destroy(hitup.collider.gameObject);
                                Instantiate(Diamond,
                                    new Vector2(gameObject.transform.position.x,
                                        gameObject.transform.position.y - 1),
                                    Quaternion.identity);
                            }
                            
                            break;
                    }
                }

                timer = 0.1875f;
            }
        }
        else //If duration expired
        {
            GetComponent<Animator>().enabled = false;
            wallRenderer = GetComponent<SpriteRenderer>();
            wallRenderer.sprite = wallSprite;
            if (activated)
            {
                SoundManager.Instance.StopAllAudio();
                activated = false;
            }
            
        }
    }
}




