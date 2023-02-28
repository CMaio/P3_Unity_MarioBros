using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAdapter : MonoBehaviour
{

    [SerializeField] private Rigidbody bridgeRB;
    [SerializeField] private float bridgeForce;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Bridge"))
        {
            bridgeRB.AddForceAtPosition(-hit.normal * bridgeForce, hit.point);
        }
    }
}
