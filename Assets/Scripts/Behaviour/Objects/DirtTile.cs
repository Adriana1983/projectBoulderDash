using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Behaviour.Objects
{
    public class DirtTile : MonoBehaviour
    {
        public GameObject tilemapGameObject;
        Tilemap tilemap;
        
        void Start()
        {
            if (tilemapGameObject != null)
            {
                tilemap = tilemapGameObject.GetComponent<Tilemap>();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
//        void OnCollisionEnter2D(Collision2D collision)
//        {
//            Debug.Log(collision);
//            Vector3 hitPosition = Vector3.zero;
//            if (tilemap != null && tilemapGameObject == collision.gameObject)
//            {
//                ContactPoint2D hit = collision.GetContact(0);
//                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
//                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
//                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
//            }
//        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other);
            Vector3 hitPosition = Vector3.zero;
            
//            ContactPoint2D hit = other.GetContact(0);
//            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
//            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
//            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }
}
