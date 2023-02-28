using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] LifeManager lifeManager;
    [SerializeField] AudioSource audioS;
    bool triggered;
    [SerializeField] GameObject particles;


    private void Awake()
    {
        if(lifeManager == null)
        {
            lifeManager = GameObject.Find("SuperMario").GetComponent<LifeManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!lifeManager.haveAllHealth() && life != null && !triggered && other.gameObject.GetComponent<MarioPlayerController>() != null)
        {
            audioS.Play();
            life.life();
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

