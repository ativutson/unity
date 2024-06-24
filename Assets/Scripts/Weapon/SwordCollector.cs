using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollector : MonoBehaviour
{
    public bool hasSword = false;
    private GameObject wieldSword;
    private GameObject rightHand;

    // Start is called before the first frame update
    void Start()
    {
        wieldSword = GameObject.Find("LongSwordWield");
        wieldSword.SetActive(hasSword);
    }

    // Update is called once per frame
    void Update()
    {
        wieldSword.SetActive(hasSword);
    }

    public void ReceiveSword()
    {
        hasSword = true;
    }
}
