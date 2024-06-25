using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundPlayer : MonoBehaviour
{
    public AudioClip clip;
    Animator animator;
    private float _lastFootstep;

    void OnValidate(){
        if(!animator) animator = GetComponent<Animator>();
    }
    private void Update(){
        var footstep = animator.GetFloat("Footstep");
        //Debug.Log(footstep);
        //Debug.Log("last " + _lastFootstep);
        if(Mathf.Abs(footstep) < .00001f){
            footstep = 0;
        }
        if(_lastFootstep > 0 && footstep < 0 || _lastFootstep < 0 && footstep > 0){
            PlayClipAtPoint(clip, transform.position);
        }

        _lastFootstep = footstep;
    }

    // copies audiosource properties to temp audiosource for playing at a position
    public static AudioSource PlayClipAtPoint(AudioClip audioSource, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        tempASource.clip = audioSource;
        tempASource.volume = 0.15f;  
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }
}
