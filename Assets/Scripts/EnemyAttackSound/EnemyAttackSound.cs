using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSound : MonoBehaviour
{
    public AudioClip clip;
    
    public void play_attack_sound(){
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
