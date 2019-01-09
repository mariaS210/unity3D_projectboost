using System;
using System.Collections;

namespace UnityEngine
{
    public static class AudioSourceExtensions
    {
        public static bool isFading;
        public static void FadeOut(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeOutCore(a, duration));
        }
     
        public static void FadeIn(this AudioSource a, float duration)
        {
            a.GetComponent<MonoBehaviour>().StartCoroutine(FadeInCore(a, duration));
        }
     
        private static IEnumerator FadeOutCore(AudioSource audioSource, float duration)
        {
            float startVolume = audioSource.volume;
            float startTime = Time.realtimeSinceStartup;
            float endTime = startTime + duration;
            while (isFading)
            {
                yield return new WaitForSeconds(0.1f);
            }
            isFading = true;

            while ((audioSource.volume - Mathf.Epsilon) >= 0)
            {
                float currentTime = Time.realtimeSinceStartup - startTime;
                if (currentTime > endTime)
                {
                    Debug.Log("Overtime Fade Out");
                    break;
                }
                audioSource.volume -= currentTime / duration;
                //Debug.Log("fadeout" + audioSource.volume);
                yield return new WaitForEndOfFrame();
            }
            audioSource.Stop();
            audioSource.volume = startVolume;
            isFading = false;
        }
     
        private static IEnumerator FadeInCore(AudioSource audioSource, float duration)
        {
            float startVolume = 0.2f;
            //if (isFading)
            //{ startVolume = audioSource.volume; }
            float startTime = Time.realtimeSinceStartup;
            float endTime = startTime + duration;

            audioSource.volume = 0;
            audioSource.Play();
     
            while ((audioSource.volume + Mathf.Epsilon) < 1.0f)
            {
                float currentTime = Time.realtimeSinceStartup - startTime;
                if (currentTime > endTime)
                {
                    Debug.Log("Overtime Fade In");
                    break;
                }
                audioSource.volume += startVolume * currentTime / duration;
                //Debug.Log("fadein" + audioSource.volume);
                yield return new WaitForEndOfFrame();
            }
     
            audioSource.volume = 1f;
            isFading = false;
        }
    }
}