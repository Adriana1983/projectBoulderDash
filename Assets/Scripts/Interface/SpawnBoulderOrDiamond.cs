using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;

public class SpawnBoulderOrDiamond: MonoBehaviour
{
    public GameObject Boulder;
    public GameObject Diamond;
    private GameObject Lastspawner;
    private int count;
    private float spawnTimer;
    private Random random = new Random();
    private Random randomboulderofdiamond = new Random();

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 0.2f)
        {
            GameObject newspawn = Lastspawner;
            GameObject[] List = GameObject.FindGameObjectsWithTag("BoulderSpawner");
            random.Next(0, List.Length);
            GameObject Spawnposition = List[random.Next(0, List.Length)];
            Lastspawner = Spawnposition;
            if (newspawn != Spawnposition || count == 0)
            {
                if (randomboulderofdiamond.Next(-1, 1) == 0)
                {
                    Instantiate(Boulder,
                        new Vector3(Spawnposition.transform.position.x, Spawnposition.transform.position.y - 1,
                            Spawnposition.transform.position.z + 1),
                        Quaternion.identity);
                    spawnTimer = 0;
                }
                else
                {
                    Instantiate(Diamond,
                        new Vector3(Spawnposition.transform.position.x, Spawnposition.transform.position.y - 1,
                            Spawnposition.transform.position.z + 1),
                        Quaternion.identity);
                    spawnTimer = 0;
                }

                count++;
            }
        }
    }
}
