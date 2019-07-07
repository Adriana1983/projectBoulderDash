using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Behaviour.Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Behaviour.Player
{
    public class Movement : MonoBehaviour
    {
        private bool finished;
        public float timerNextScene = 5;

        //rockfort movement speed
        public float speed = 7.5f;
        public string hitDirection;
        public string inputGot;
        public bool isHit;

        public Vector3 lastPos;
        public Vector3 targetPos;
        public bool mustMove;
        public bool isMoving;

        public int animationDirection = 0;
        public bool isIdle;
        public Animator animator;
        public float time;

        public LayerMask layer;
        public BoxCollider2D ghost;

        //Camera concept 01-07-2019
        GameObject Camera;
        Vector3 targetCamPos;

        //Animation direction clockwise
        enum Direction
        {
            Idle = 0,
            Up = 1,
            Right = 2,
            Down = 3,
            Left = 4
        }

        private void Start()
        {
            #region possible new cave-camera 1
            //Camera concept 01-07-2019
            Camera = GameObject.FindGameObjectWithTag("MainCamera");
            //Verplaats camera op rockford
            Camera.transform.position = transform.position;
            //Haal camera na voor
            Camera.transform.position += Vector3.back;
            //Zet camera doel gelijk aan huidig positie
            targetCamPos = Camera.transform.position;
            #endregion

            targetPos = transform.position;
            isMoving = false;
            mustMove = false;
            hitDirection = "";

            //Added ghost collider to prefent boulders from falling while Rockford is not yet in target position (this couldn't be fixed with a LateUpdate!)
            //Remove Rockford as parent from ghost collider to have it work as a gameobject but still have it appear in the direction of Rockford's movement
            ghost.gameObject.transform.parent = null;
            ghost.enabled = false;
        }

        //this List & method hold all pressed keys - it is created for the purpose to give priority to the last pressed key to move Rockford
        List<KeyCode> inputs = new List<KeyCode>();
        void OnGUI()
        {
            Event e = Event.current;
            //check if a key is pressed
            if (e.isKey && e.type == EventType.KeyDown)
            {
                if (!inputs.Contains(e.keyCode))
                    inputs.Insert(0, e.keyCode); // add new key to top of list
            }
            //check if key is released
            else if (e.isKey && e.type == EventType.KeyUp)
            {
                inputs.Remove(e.keyCode);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (Input.GetKey(KeyCode.Escape))
            {
                //add explosion
                Score.Instance.RockfordDies();
            }


            lastPos = transform.position;

            inputGot = Input.inputString;
            if (inputGot.Length < 1)
            {
                inputGot = inputGot.ToUpper();
            }
            else
            {
                inputGot = char.ToUpper(inputGot[0]) + inputGot.Substring(1);
            }

            isHit = false;

            //Player isn't moving, allow movement
            if (!isMoving && !finished  && Score.Instance.caveTime > 0)
            {
                //Variable for requested player direction
                Vector3 targetDirection = Vector3.zero;

                if (inputs.Count > 0)
                {
                    //changed this code to give priority to the last pressed key to move Rockford in that direction
                    //previous code gave priority to moving to the left

                    if (inputs[0] == KeyCode.A || inputs[0] == KeyCode.LeftArrow)
                    {
                        mustMove = true;
                        targetDirection = Vector3.left;
                        animationDirection = (int)Direction.Left;
                    }
                    else if (inputs[0] == KeyCode.D || inputs[0] == KeyCode.RightArrow)
                    {
                        mustMove = true;
                        targetDirection = Vector3.right;
                        animationDirection = (int)Direction.Right;
                    }
                    else if (inputs[0] == KeyCode.W || inputs[0] == KeyCode.UpArrow)
                    {
                        mustMove = true;
                        targetDirection = Vector3.up;
                        animationDirection = (int)Direction.Up;
                    }
                    else if (inputs[0] == KeyCode.S || inputs[0] == KeyCode.DownArrow)
                    {
                        mustMove = true;
                        targetDirection = Vector3.down;
                        animationDirection = (int)Direction.Down;
                    }
                }

                //Check for collision in requested player direction
                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 1, layer);

                //Did we hit anything?
                if (hit.collider != null)
                {
                    //What did we hit?
                    switch (hit.collider.gameObject.tag)
                    {
                        //We hit a boulder
                        case "Boulder":
                            //can't move boulders in stay in place mode
                            if (!Input.GetKey(KeyCode.LeftControl))
                                //call boulder hit function, decides if the player can move the boulder
                                mustMove = hit.collider.gameObject.GetComponent<Boulder>().BoulderHit(targetDirection);
                            else
                                mustMove = false;
                            break;
                        //we hit dirt
                        case "Dirt":
                            {
                                //Allow player to move
                                mustMove = true;
                                //Delete the dirt
                                SoundManager.Instance.PlayWalkDirt();
                                //Get reference to tilemap
                                var map = hit.collider.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>();
                                //Delete tile
                                if (map != null)
                                    map.SetTile(map.WorldToCell(targetPos + targetDirection), null);
                            }
                            break;
                        //we hit diamond
                        case "Diamond":
                            mustMove = true;
                            SoundManager.Instance.PlayCollectdiamond();
                            Destroy(hit.collider.gameObject);

                            if (Score.Instance.diamondsCollected < Score.Instance.diamondsNeeded)
                            {
                                //counting score by counting diamond value of current cave & current difficulty level
                                Score.Instance.TotalScore += Score.Instance.initialDiamondsValue;
                            }
                            else
                            {
                                Score.Instance.TotalScore += Score.Instance.extraDiamondsValue;
                            }
                            //counting collected diamonds
                            Score.Instance.diamondsCollected++;
                            break;
                        //exit opens when required amound of diamond have been collected - if it's not open it behaves as Titanium Wall (a.k.a. bounds)
                        case "Exitdoor":
                            mustMove = false;
                            if (Score.Instance.diamondsCollected >= Score.Instance.diamondsNeeded)
                            {
                                mustMove = true;

                                SoundManager.Instance.PlayFinished();//this sound is play upon completing cave/intermission the remaining time is turned into score (1 point per second)
                                //destroy exit when Rockford enter the position of exit
                                Destroy(hit.collider.gameObject);

                                finished = true;
                                Score.Instance.Finish = true;
                            }
                            break;
                        //we hit something else, player cannot move
                        default:
                            mustMove = false;
                            break;
                    }
                }

                //Did the player have to move?
                if (mustMove)
                {
                    //Check for stay in place mode, else allow player movement
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        if (hit.collider == null)
                            SoundManager.Instance.PlayWalkEmpty();

                        targetPos += targetDirection;
                        ghost.transform.position = targetPos;
                        ghost.enabled = true;

                        #region possible new cave-camera 2
                        //Camera concept 01-07-2019
                        var distance = transform.position - Camera.transform.position;
                        if ((distance.x < -10 && animationDirection == (int)Direction.Left) || (distance.x > 10 && animationDirection == (int)Direction.Right))
                        {
                            targetCamPos += targetDirection;
                        }

                        if ((distance.y < -1 && animationDirection == (int)Direction.Down) || (distance.y > 1 && animationDirection == (int)Direction.Up))
                        {
                            targetCamPos += targetDirection;
                        }
                        #endregion

                        animator.SetInteger("AnimationDirection", animationDirection);

                        animator.SetBool("isMoving", true);
                        isMoving = true;
                    }
                }
                else
                {
                    animator.SetInteger("AnimationDirection", 0);
                    Idle();
                }
            }
            //Player is still moving
            else
            {
                //The Current Position = Move To (the current position to the new position by the speed * Time.DeltaTime)
                if (isMoving)
                {
                    time = 0;

                    transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                    #region possible new cave-camera 3
                    //Camera concept 01-07-2019
                    Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, targetCamPos, speed * Time.deltaTime);
                    #endregion
                }

                //Wait for movement to finish
                if (transform.position == targetPos)
                {
                    ghost.enabled = false;
                    isMoving = false;
                    mustMove = false;

                    //check if the level has been finished
                    if (finished == true)
                    {
                        //remaining time turns into score
                        if (Score.Instance.caveTime > 0)
                        {
                            Score.Instance.caveTime--;

                            if (Score.Instance.caveTime < 0) Score.Instance.caveTime = 0;

                            Score.Instance.TotalScore++;
                        }
                        else
                        {
                            var caveloader = GameObject.FindObjectOfType<CaveLoader>();

                            if (caveloader.FillScreen() == 0)
                            {
                                //Change scene

                                Score.Instance.NextCave();
                                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            }
                        }
                    }
                }
            }
        }

        public void Idle()
        {
            time += Time.deltaTime;

            if (time > 10.0f)
            {
                animator.SetInteger("idle", 2);
            }
            else if (time > 5.0f)
            {
                animator.SetInteger("idle", 1);
            }
            else
            {
                animator.SetInteger("idle", 0);
            }
            isIdle = true;
            animator.SetBool("isMoving", isMoving);

        }

        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}