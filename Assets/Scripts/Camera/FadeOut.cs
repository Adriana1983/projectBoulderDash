using System;
using System.Collections;
using Behaviour.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Camera
{
    public class FadeOut : MonoBehaviour 
    {
        #region FIELDS
        public Image fadeOutUIImage;
        public float fadeSpeed = 0.8f;


        public enum FadeDirection
        {
            In, //Alpha = 1
            Out // Alpha = 0
        }

        #endregion
        #region MONOBHEAVIOR
//
//        public void Start()
//        {
//            gameObject.SetActive(false);
//        }

        public void Execute()
        {
            StartCoroutine(Fade(FadeDirection.In));
        }

        #endregion
        #region FADE
        private IEnumerator Fade(FadeDirection fadeDirection) 
        {
            float alpha = (fadeDirection == FadeDirection.Out)? 0.95f : 0;
            float fadeEndValue = (fadeDirection == FadeDirection.Out)? 0 : 0.95f;
            if (fadeDirection == FadeDirection.Out) {
                while (alpha >= fadeEndValue)
                {
                    SetColorImage (ref alpha, fadeDirection);
                    yield return null;
                }
                fadeOutUIImage.enabled = false; 
            } else {
                fadeOutUIImage.enabled = true; 
                while (alpha <= fadeEndValue)
                {
                    SetColorImage (ref alpha, fadeDirection);
                    yield return null;
                }
            }
        }
        #endregion
        #region HELPERS
        public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad) 
        {
            yield return Fade(fadeDirection);
            SceneManager.LoadScene(sceneToLoad);
        }
        private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
        {
            fadeOutUIImage.color = new Color (fadeOutUIImage.color.r,fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
            alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out)? -1 : 1) ;
        }
        #endregion
    }
}
