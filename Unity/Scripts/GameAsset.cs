using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    private static GameAsset _i;


    public static GameAsset i{
        get{
            if(_i == null ) _i = Instantiate(Resources.Load<GameAsset>("GameAsset"));
            return _i;
        }
    }

    public SoundAudioClip[] soundAudioClipsArray; 




    [System.Serializable]
      public class SoundAudioClip{
        public SoundManager.Sound sound;
        public AudioClip audioClip;




      }
    
}   