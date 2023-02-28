using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMusic : MonoBehaviour
{
    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip step;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip die;
    
    public void StepSound()
    {
        audioS.clip = step;
        audioS.Play();
    }

    public void JumpSound()
    {
        audioS.clip = jump;
        audioS.Play();
    }

    public void DieSound()
    {
        audioS.clip = die;
        audioS.Play();
    }


}