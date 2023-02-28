using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageReciver : MonoBehaviour
{
    [SerializeField] EnemyHealth em;
    [SerializeField] bool head;
    [SerializeField] float damage;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entra aqui");
        if (other.gameObject.TryGetComponent(out MarioPlayerController mario))
        {
            if (head) { em.doDamage(em.maxDamage); }
            else { em.doDamage(damage); }
        }
    }
}
