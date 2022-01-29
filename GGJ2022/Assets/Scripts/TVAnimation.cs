using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class TVAnimation : MonoBehaviour
{

    VideoPlayer videoPlayer;
    public Sprite breakingNewsImage;
    bool isNoise = false;
    bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {

        /*videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Prepare();

        GetComponent<Image>().material.renderQueue = 3000;*/
        //videoPlayer.Play();

        GetComponent<Image>().material.SetColor("_BaseColor", new Color(1, 1, 1));
        GetComponent<Image>().material.SetTexture("_BaseMap", breakingNewsImage.texture);
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        source.Play();
        isPlaying = true;
        
    }

    public void StopPlayback()
    {
        videoPlayer.Stop();
        GetComponent<Image>().material.SetColor("_BaseColor", new Color(0,0,0));
        GetComponent<Image>().material.SetTexture("_BaseMap", null);
        //if (isNoise == false) SetMovie();
        //isPlaying = false;
    }


    public void SetMovie()
    {
        GetComponent<Image>().material.SetTexture("_BaseMap", breakingNewsImage.texture);
        GetComponent<Image>().material.SetColor("_BaseColor", new Color(1, 1, 1));
    }

    public void SetNoise()
    {
        isNoise = true;
    }

    public bool GetNoise()
    {
        return isNoise;
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
        if (videoPlayer != null)
        {

            if (videoPlayer.texture != null && videoPlayer.isPlaying) GetComponent<Image>().material.SetTexture("_BaseMap", videoPlayer.texture);
        }
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
