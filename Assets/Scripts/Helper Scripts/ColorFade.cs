using System.Collections;
using Camera;
using UnityEngine;
using UnityEngine.UI;

namespace Helper_Scripts
{
    public class ColorFade : MonoBehaviour
    {
        public bool isActive;
      
        public Shader shader;
        public RawImage blackScreen;
        public float time;
        
        public void Start()
        {
            isActive = false;
        }

        public void Update()
        {
            if (isActive)
            {
                StartCoroutine(Fade());
                blackScreen.CrossFadeAlpha (1.0f, time, true);
            }
        }
        
        private IEnumerator Fade() 
        {
            gameObject.GetComponent<UnityEngine.Camera>().SetReplacementShader(shader, "RenderType");
            yield return null;
        }
        
        private IEnumerator FadeToBlack() 
        {
            blackScreen.color = Color.black;
            blackScreen.canvasRenderer.SetAlpha(0.2f);
            while (blackScreen.color.a < 1.0f)
            {
                
                yield return null;
            }
        }

    }
}
