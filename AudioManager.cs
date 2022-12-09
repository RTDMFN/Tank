using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] Sounds;

    void Awake(){
        foreach(Sound s in Sounds){
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
        }
    }

    public void Play(string name){
        foreach(Sound s in Sounds){
            if(s.name.Equals(name)){
                s.audioSource.Play();
            }
            
        }
    }

}
