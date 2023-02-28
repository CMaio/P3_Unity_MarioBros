using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cajaEjemploInterficie : MonoBehaviour, IRestartGame
{
    [SerializeField] private Transform initPos;
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        gameManager.addRestartListener(this);
    }
    private void OnDisable()
    {
        gameManager.removeRestartListener(this);
    }
    public void RestartGame()
    {
        transform.position = initPos.position;
        transform.rotation = initPos.rotation;
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
