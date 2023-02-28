using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Trigger : MonoBehaviour
{
    [SerializeField] float maxAttachingAngle = 20;
    GameObject attachedMario;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MarioPlayerController>() != null)
        {
            attachMario(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MarioPlayerController>() != null)
        {
            dettachMario();
        }
    }

    private void Update()
    {
        if (180.0f > Vector3.Angle(transform.up, Vector3.forward) && Vector3.Angle(transform.up, Vector3.forward) > maxAttachingAngle)
        {
            dettachMario();
        }

    }
    private void attachMario(GameObject mario)
    {
        attachedMario = mario;
        mario.transform.parent = transform;
    }

    private void dettachMario()
    {
        if (attachedMario != null)
        {
            attachedMario.transform.parent = null;
            attachedMario = null;
        }

    }
}
