using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSound : MonoBehaviour
{
    public AudioClip clip;
    Animator animator;
    private float _lastAttack;

    void OnValidate(){
        if(!animator) animator = GetComponent<Animator>();
    }
    private void Update(){
        var attack = animator.GetFloat("Attack");
        Debug.Log("attack " + attack);
        Debug.Log("last attack " + _lastAttack);
        if(Mathf.Abs(attack) < .00001f){
            attack = 0;
        }
        if(_lastAttack > 0 && attack < 0 || _lastAttack < 0 && attack > 0){
            PlayClipAtPoint(clip, transform.position);
        }

        _lastAttack = attack;
    }

    // copies audiosource properties to temp audiosource for playing at a position
    public static AudioSource PlayClipAtPoint(AudioClip audioSource, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        tempASource.clip = audioSource;
        tempASource.volume = 0.5f;
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }
}
