using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchController : MonoBehaviour
{
    [SerializeField] private BoxCollider punchCollider;
    bool hit;
    public void setPunch(bool change)
    {
        punchCollider.enabled = change;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy goomba))
        {
            goomba.setHitState();
            goomba.GetComponent<EnemyHealth>().doDamage(50);
        }
    }

}
