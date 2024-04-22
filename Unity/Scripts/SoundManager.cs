using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager 
{
    

    public enum Sound{
        BackgroundNoise,
        Goal,
        Miss,
        BallHit,
        Air,
        Whistle,
        Error,

    }



    private static Dictionary<Sound,float> soundTimeDictionary;

    private static GameObject oneShotGameObj;
    private static AudioSource oneShotAudioSrc;
    public static void Initialize(){
        soundTimeDictionary = new Dictionary<Sound, float>();
        soundTimeDictionary[Sound.BackgroundNoise] = 0f;
    }
    public static void PlaySound3D(Sound sound, Vector3 position){
        if(CanPlaySound(sound)){
            GameObject soundGameObj = new GameObject("Sound");
            soundGameObj.transform.position = position;
            AudioSource audioSorce = soundGameObj.AddComponent<AudioSource>();
            audioSorce.clip = GetAudioClip(sound);
            audioSorce.Play();



             Object.Destroy(soundGameObj,audioSorce.clip.length);
        }

       
    }


    public static void PlaySound2D(Sound sound){
        if(CanPlaySound(sound)){
            
            if(oneShotGameObj == null){
            oneShotGameObj= new GameObject("Sound");
            oneShotAudioSrc = oneShotGameObj.AddComponent<AudioSource>();
            }
            oneShotAudioSrc.PlayOneShot(GetAudioClip(sound));
        }
    }
    
    private static bool CanPlaySound(Sound sound){
        switch (sound){
            default:
                return true;
            case Sound.BackgroundNoise:
                if(soundTimeDictionary.ContainsKey(sound)){
                    float lastTimePlayed = soundTimeDictionary[sound];
                    float backgroundPlayTimerMax = 60;
                    if(lastTimePlayed + backgroundPlayTimerMax < Time.time){
                            soundTimeDictionary[sound] = Time.time;
                            return true;
                    }else{
                        return false;
                    }
                }else{
                    return true;
                }
               // break;    
        }
    }
    
    private static AudioClip GetAudioClip(Sound sound){
        foreach(GameAsset.SoundAudioClip soundAudioClip in GameAsset.i.soundAudioClipsArray){
            if(soundAudioClip.sound == sound){
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + " not found");
        return null;
    }
}
