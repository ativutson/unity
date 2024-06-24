using UnityEngine;

public class Collision : MonoBehaviour
{
    public GameObject lossTextObject;
    
    void Start()
    {
        lossTextObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            lossTextObject.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
