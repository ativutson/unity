using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSound : MonoBehaviour
{
    public AudioClip attack_clip;
    public AudioClip run_clip;


    public void play_attack_sound(){
        AudioSource.PlayClipAtPoint(attack_clip, transform.position, 1f);
    }

    public void play_run_sound()
    {
        AudioSource.PlayClipAtPoint(run_clip, transform.position, 1f);
        
    }
    // audio from
    // https://www.fesliyanstudios.com/royalty-free-sound-effects-download/footsteps-on-grass-284
}
