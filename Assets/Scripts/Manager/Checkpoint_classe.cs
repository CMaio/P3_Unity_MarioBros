using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_classe : MonoBehaviour
{

    [SerializeField] private Transform initPos;
    [SerializeField] private int index;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out MarioPlayerController mario))
        {
            mario.setCheckpoint(this);
        }
    }
    public int getIndex() { return index; }
    public Transform getCheckpointTransform() 
    {
        return initPos;
    }
}
