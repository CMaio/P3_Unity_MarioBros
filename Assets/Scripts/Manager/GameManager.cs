using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<IRestartGame> listeners = new List<IRestartGame>();
    bool died = false;
    [SerializeField] AudioSource audioS;
    [SerializeField] AudioClip play;
    [SerializeField] AudioClip die;

    public void RestartGame()
    {
        audioS.clip = play;
        audioS.Play();
        foreach (IRestartGame l in listeners)
        {
            l.RestartGame();
        }
        died = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && died)
        {
            RestartGame();
        }
    }
    public void addRestartListener(IRestartGame listener)
    {
        listeners.Add(listener);
    }
    public void removeRestartListener(IRestartGame listener)
    {
        listeners.Remove(listener);
    }

    public void playerDie()
    {
        audioS.Stop();  
        audioS.clip = die;
        audioS.Play();
        foreach (IRestartGame l in listeners)
        {
            l.Die();
        }
        died = true;
    }
}
