using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VignetteAnimation : MonoBehaviour
{
    VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        /*videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;

        GetComponent<Image>().material.renderQueue = 3001;*/
        //videoPlayer.Play();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        source.Play();

    }

    public void StopPlayback()
    {
        videoPlayer.Stop();
    }

    public void InitPlayback()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();

        GetComponent<Image>().material.renderQueue = 3000;
    }


    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.texture != null && videoPlayer.isPlaying)
            //Texture2D bumpTexture = source.texture;// toTexture2D(videoPlayer.targetTexture);
            GetComponent<Image>().material.SetTexture("_BaseMap", videoPlayer.texture);
        
        //GetComponent<Image>().material.SetTexture("_ParallaxMap", videoPlayer.texture);
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
