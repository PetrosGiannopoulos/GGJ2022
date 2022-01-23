using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class RippleAnimation : MonoBehaviour
{

    VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        //videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;

        
        //videoPlayer.Play();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        source.Play();
        
    }

    
    

    // Update is called once per frame
    void Update()
    {
        if(videoPlayer.texture!=null)
        //Texture2D bumpTexture = source.texture;// toTexture2D(videoPlayer.targetTexture);
        GetComponent<Image>().material.SetTexture("_BumpMap", videoPlayer.texture);
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
