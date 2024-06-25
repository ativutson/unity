using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSoundFX : MonoBehaviour
{
    public static HealthSoundFX instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake(){
        if(instance == null){
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume){
        //spawn in game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        //asign clip
        audioSource.clip = audioClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of clip
        float clipLength = audioSource.clip.length;
        //destroy gameobject when done playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
