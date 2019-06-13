using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using UnityEngine;

public class SpawnBoulder : MonoBehaviour
{
    public GameObject Boulder;
    private float spawnTimer;
    private Random random = new Random();

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 0.2f)
        {
            GameObject[] List = GameObject.FindGameObjectsWithTag("BoulderSpawner");
            random.Next(0, List.Length);
            GameObject Spawnposition = List[random.Next(0, List.Length)];
            Instantiate(Boulder,
                    new Vector3(Spawnposition.transform.position.x, Spawnposition.transform.position.y - 1,
                        Spawnposition.transform.position.z + 1),
                    Quaternion.identity);
            spawnTimer = 0;
        }
    }
}
