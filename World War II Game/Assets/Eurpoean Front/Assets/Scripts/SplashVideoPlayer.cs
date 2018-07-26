using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

namespace xrayhunter.SplashScreen
{
    [System.Serializable]
    public class SplashVideoProperties
    {
        public VideoClip clip;

        [Range(0, 100)]
        public float volume;
    }

    [RequireComponent(typeof(VideoPlayer))]
    public class SplashVideoPlayer : MonoBehaviour
    {

        public int afterLoadScene = 0;

        public SplashVideoProperties[] videos;
        
        private VideoPlayer video;

        private int videoCounter = -1;

        private void Start()
        {
            video = GetComponent<VideoPlayer>();
            videoCounter++;
            video.loopPointReached += Video_loopPointReached;
        }

        private void Video_loopPointReached(VideoPlayer source)
        {
            videoCounter++;
            Debug.Log(videoCounter);
            if (videos.Length > videoCounter && videos[videoCounter] != null && videos[videoCounter].clip != null)
            {
                source.clip = videos[videoCounter].clip;
                source.Play();
            }
            else
            {
                if (afterLoadScene != -1)
                {
                    // Load After scene
                    SceneManager.LoadScene(afterLoadScene);
                }
                return;
            }
        }
    }

}