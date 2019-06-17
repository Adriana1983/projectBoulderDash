using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Helper_Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Camera
{
    public class Grayscale : MonoBehaviour {
        public Material material1;
        public Material material2;
        public float duration;
        
        public void OnDestroy()
        {
            if (UnityEngine.Camera.main != null)
            {
                float lerp = Mathf.PingPong(Time.time, duration) / duration;
                
                foreach (var tilemap in GetComponents<TilemapRenderer>())
                {
                    tilemap.material.Lerp(material1, material2, lerp);
                }

                foreach (var sprite in GetComponents<SpriteRenderer>())
                {
                    sprite.material.Lerp(material1, material2, lerp);
                }
            }
        }
        
    }
}