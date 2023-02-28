using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMario : MonoBehaviour
{
    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip jump2;
    [SerializeField] AudioClip jump3;
    [SerializeField] AudioClip punch;
    [SerializeField] AudioClip punch2;
    [SerializeField] AudioClip punch3;



    public void Step()
    {
        audioS.clip = walk;
        audioS.Play();
    }

    public void Jump1()
    {
        audioS.clip = jump;
        audioS.Play();
    }

    public void Jump2()
    {
        audioS.clip = jump2;
        audioS.Play();
    }
    public void Jump3()
    {
        audioS.clip = jump3;
        audioS.Play();
    }

    public void PunchSound1()
    {
        audioS.clip = punch;
        audioS.Play();
    }

    public void PunchSound2()
    {
        audioS.clip = punch2;
        audioS.Play();
    }

    public void PunchSound3()
    {
        audioS.clip = punch3;
        audioS.Play();
    }


}
