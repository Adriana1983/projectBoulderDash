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
    private float timer = 0.15f;
    private SpriteRenderer wallRenderer;
    
    private void Update()
    {
        //Als er een boulder een Magic wall heeft geactiveerd doe dit
        if (activated)
        {
            //Zoekt alle andere magic walls en zet hun variable activated naar true
            foreach (var variable in GameObject.FindGameObjectsWithTag("MagicWall"))
            {
                variable.GetComponent<MagicWall>().activated = true;
            }
            //Verandert sprite naar Magic wall moet worden verplaatst met animaties
            wallRenderer= GetComponent<SpriteRenderer>(); 
            wallRenderer.sprite = magicWallSprite;
            activeDuration = 0.15f;
            
            //Start countdown van de duration
            if (activeDuration >= 0)
            {
                activeDuration -= Time.deltaTime;                
            }
        }
        //if duration is still active
        if (activeDuration > 0)
        {
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

                timer = 0.15f;
            }
        }
        else //If duration expired
        {
            wallRenderer = GetComponent<SpriteRenderer>();
            wallRenderer.sprite = wallSprite;
        }
    }
}




