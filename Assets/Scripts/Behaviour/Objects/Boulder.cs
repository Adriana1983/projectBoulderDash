using UnityEngine;
using Random = System.Random;

namespace Behaviour.Objects
{
    public class Boulder : MonoBehaviour
    {
        private float timer = 0.1875f; //time betwee actions
        private float timer2 = 0.1f;
        Random random = new Random();
        public bool falling;
        bool moving = false;

        //Magic wall variables
        public bool activatedWall;
        public bool lastpositionFalling;

        public GameObject explosion;

        //Return bool decides if rockford is allowed to move a boulder that can be moved
        public bool BoulderHit(Vector3 targetDirection)
        {
            //This makes sure that the code in the if rns only once every tenth of a second
            timer2 -= Time.deltaTime;
            if (timer2 < 0)
            {
                timer2 = 0.1f;
                //1 - 0.125 ^ Time.deltaTime;
                if (random.Next(1, 8) == 1)
                {
                    moving = true;
                }
                else
                {
                    return false;
                }

                //Rockford can't move boulders up or down
                if (targetDirection == Vector3.up || targetDirection == Vector3.down)
                    return false;

                //Check if there is something behind the boulder in the direction we want to push
                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, 1);
                if (hit.collider != null)
                    //There is something in the way, rockford cannot push this boulder
                    return false;

                //There was nothing, so move the boulder to the empty square
                SoundManager.Instance.PlayBox_push();
                transform.position += targetDirection;
                return true;                
            }
            return false;
        }

        void Update()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                //check if there is anything below this boulder (transform is this boulder)
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1);

                if (hit.collider == null)
                {
                    //there's nothing beneath this boulder
                    transform.position += Vector3.down;
                    falling = true; //this boolean is used to remember if the boulder is falling
                }
                else
                {
                    //there is something beneath this boulder
                    switch (hit.collider.tag)
                    {
                        case "MagicWall":
                            if (lastpositionFalling || falling)
                            {
                                if (activatedWall == false)
                                {
                                    hit.collider.gameObject.GetComponent<MagicWall>().activated = true;
                                    activatedWall = true;
                                }
                            }
                            break;
                        case "Dirt":
                            if (falling)
                                SoundManager.Instance.PlayBoulder();
                            break;

                        case "Wall":
                        case "Diamond":
                        case "Boulder":

                            if (falling)
                                SoundManager.Instance.PlayBoulder();

                            //don't test collision on falling things
                            if (hit.collider.tag == "Boulder" && hit.collider.gameObject.GetComponent<Boulder>().falling) break;
                            if (hit.collider.tag == "Diamond" && hit.collider.gameObject.GetComponent<Diamond>().falling) break;

                            //check space left and right of hit object
                            RaycastHit2D hit_left = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.left, 1);
                            RaycastHit2D hit_right = Physics2D.Raycast(hit.point + new Vector2(0, -0.5f), Vector2.right, 1);

                            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 1);
                            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 1);

                            if (hit_left.collider == null && hit_right.collider == null && left.collider == null && right.collider == null)
                            {
                                //random left or right
                                if (random.Next(0, 1) == 0)
                                {
                                    transform.position = transform.position + Vector3.left;
                                    falling = true;
                                }
                                else
                                {
                                    transform.position = transform.position + Vector3.right;
                                    falling = true;
                                }
                            }
                            //left space is empty Boulder falls left
                            else if (hit_left.collider == null && left.collider == null)
                            {
                                transform.position = transform.position + Vector3.left;
                                falling = true;
                            }
                            //right space is empty Boulder falls right
                            else if (hit_right.collider == null && right.collider == null)
                            {
                                transform.position = transform.position + Vector3.right;
                                falling = true;
                            }
                            break;

                        case "Player":
                            if (falling)
                            {
                                //player dies
                                DrawExplosion(hit);
                                Debug.Log("Player dead");
                                Destroy(hit.collider.gameObject);
                                Score.Instance.RockfordDies();
                            }
                            break;

                        case "Firefly":
                        case "Butterfly":
                            if (falling)
                            {
                                //firefly/butterfly dies
                                DrawExplosion(hit);
                                Debug.Log("Firefly/Butterfly dead");
                                Destroy(hit.collider.gameObject);
                            }
                            break;

                        default:
                            break;
                    }
                    lastpositionFalling = falling;
                    falling = false;
                }

                timer = 0.1875f;
            }
        }

        public void DrawExplosion(RaycastHit2D hit)
        {
            //Draw 3x3 explosion grid
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up + Vector3.left, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.up + Vector3.right, Quaternion.identity);

            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.left, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.right, Quaternion.identity);

            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down + Vector3.left, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down, Quaternion.identity);
            GameObject.Instantiate(explosion, hit.transform.transform.position + Vector3.down + Vector3.right, Quaternion.identity);

            SoundManager.Instance.PlayExplosion();
        }
    }
}
