using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public AudioClip clip;

    void OnTriggerEnter(Collider c) {
        if(c.gameObject != null){
            HealthCollector hc = c.gameObject.GetComponent<HealthCollector>();
            if(hc != null){
                hc.ReceiveHealth();
                AudioSource.PlayClipAtPoint(clip, c.transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    // copies audiosource properties to temp audiosource for playing at a position
    public static AudioSource PlayClipAtPoint(AudioClip audioSource, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource tempASource = tempGO.AddComponent<AudioSource>(); // add an audio source
        tempASource.clip = audioSource;
        tempASource.volume = 0.75f;
        tempASource.Play(); // start the sound
        MonoBehaviour.Destroy(tempGO, tempASource.clip.length); // destroy object after clip duration (this will not account for whether it is set to loop)
        return tempASource; // return the AudioSource reference
    }
}
