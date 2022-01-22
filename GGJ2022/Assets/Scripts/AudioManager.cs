﻿using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    //How to play sound
    //FindObjectOfType<AudioManager>().Play("nameOfSound");

    //Better - AudioManager.instance.Play();
    //Even Better - Make it a thread

    public void Play(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Problem playing sound with name: {s.name}");

        }
        else
        {
            s.source.Stop();
            if (s.source.loop == false) s.source.PlayOneShot(s.source.clip);
            else s.source.Play();
        }
        
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s.source.isPlaying)StartCoroutine(StopGradually(s));

        //s.source.Stop();
    }

    public bool IsPlaying(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s.source.isPlaying) return true;

        return false;

    }

    public void AbruptStop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s.source.isPlaying)s.source.Stop();
    }

    IEnumerator StopGradually(Sound s)
    {

        float startVolume = s.volume;
        float step = 0.01f;

        int numberIter = (int)((float)startVolume / ((float)step));
        //Debug.Log($"NumberIter: {numberIter}");
        for(int i = 0; i < numberIter; i++)
        {
            s.volume -= step;
            s.source.volume = s.volume;
            yield return new WaitForSeconds(0.02f);
        }

        s.source.Stop();
        s.volume = startVolume;
        s.source.volume = startVolume;

    }

    public void SetVolume(float value, string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Problem setting sound for: {s.name}");
        }
        else
        {
            s.volume = value;
            s.source.volume = value;
        }
    }

    public float GetVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return 1;
        }
        else
        {
            return s.source.volume;
        }
    }

    //Attach source to specific gameObject if it doesnt have one and play sound

    public void PlayOnGameObject(GameObject go, string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Problem playing sound with name: {s.name}");

        }
        else
        {
            AudioSource source;
            if (go.GetComponent<AudioSource>() == null)
            {
                source = go.AddComponent<AudioSource>();
                source.clip = s.source.clip;
                source.volume = s.source.volume;
                source.pitch = s.source.pitch;
                source.loop = s.source.loop;
                source.spatialBlend = 0.8f;
                source.playOnAwake = false;
            }

            else source = go.GetComponent<AudioSource>();

            source.PlayOneShot(source.clip);
            
            //s.source.PlayOneShot(s.source.clip);
        }

    }
}
