﻿using System.Collections;
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
        private bool isFaded;
        
        public void Start()
        {
            isActive = false;
            isFaded = false;
        }

        public void Update()
        {
            if (isActive)
            {
                StartCoroutine(Fade());
                if (!isFaded)
                {
                    StartCoroutine(FadeToBlack());
                }
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
            blackScreen.canvasRenderer.SetAlpha(0.0f);
            blackScreen.CrossFadeAlpha (1.0f, time, false);
            if (blackScreen.color.a == 1.0f)
            {
                yield return isFaded = true;
            }
        }

    }
}