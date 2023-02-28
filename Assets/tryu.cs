using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryu : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
