using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Coin : MonoBehaviour
{
    [SerializeField] Score score;
    [SerializeField] AudioSource audioS;
    bool triggered;
    [SerializeField] GameObject particles;





    private void OnTriggerEnter(Collider other)
    {
        if (score != null && !triggered && other.gameObject.GetComponent<MarioPlayerController>() != null)
        {
            audioS.Play();
            score.score();
            triggered = true;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(particles);
            

        }

    }

    private void Update()
    {
        if (!audioS.isPlaying && triggered)
        {
            Destroy(gameObject);
        }
    }
}
