using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Helper_Scripts
{
    public class Spawner : MonoBehaviour
    {

        public GameObject obj;
        public float seconds;
        public Tilemap boulderTilemap;
        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("SpawnObject");
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        private IEnumerator SpawnObject()
        {
            for (;;)
            {
                Instantiate(obj, transform.localPosition, Quaternion.identity);
                obj.layer = LayerMask.NameToLayer("Boulder");
                yield return new WaitForSeconds(seconds);
            }
        }


    }
}
