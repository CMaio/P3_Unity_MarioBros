using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    enum STATE_IA
    {
        IDLE, PATROL, WAITING, ATTACK, HIT
    }
    [Header("IA Components")]
    [SerializeField] STATE_IA currentState = STATE_IA.IDLE;
    [SerializeField] NavMeshAgent agent;

    [Header("General variables")]
    public float maxDistanceSeePlayer;
    [SerializeField] static float damping;
    [SerializeField] bool hitPlayer;
    [SerializeField] float timeAfterHit;
    [SerializeField] bool waiting;
    [SerializeField] Animator anim;//STATE = [0 => IDLE, 1=> WALK, 2 => ATACK] 
    [SerializeField] float damageDone;

    [Header("Patrol components")]
    [SerializeField] Transform PatrolPointsPool;
    List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] float distanceToChangePointPatrol = 1;
    int nextPatrolPoint = 0;

    [Header("Player components")]
    [SerializeField] GameObject player;

    [Header("Particles")]
    [SerializeField] ParticleSystem ps;
    



    void Awake()
    {
        hitPlayer = false;
        waiting = false;
        if(player == null) { player = GameObject.Find("SuperMario"); }
        foreach (Transform child in PatrolPointsPool)
        {
            patrolPoints.Add(child);
        }
    }

    void setIdleState(){ anim.SetInteger("State", 0); agent.isStopped = true; currentState = STATE_IA.IDLE; ps.Stop(); }
    void setPatrolState() { agent.isStopped = false; anim.SetInteger("State", 1); currentState = STATE_IA.PATROL; ps.startSpeed = 0.7f; ps.Play(); }
    void setWaitingState() { anim.SetInteger("State", 3); agent.isStopped = true; currentState = STATE_IA.WAITING; ps.Stop(); }
    void setAttackState() { anim.SetInteger("State", 2); currentState = STATE_IA.ATTACK; ps.startSpeed = 1.3f; ps.Play(); }
    public void setHitState() { currentState = STATE_IA.HIT; }
 
    void Update()
    {
        switch (currentState)
        {
            case STATE_IA.IDLE:
                updateIdle();
                break;
            case STATE_IA.PATROL:
                updatePatrol();
                break;
            case STATE_IA.WAITING:
                updateWaiting();
                break;
            case STATE_IA.ATTACK:
                updateAttack();
                break;
            case STATE_IA.HIT:
                updateHit();
                break;
            default:
                updateIdle();
                break;
        }
    }

    void updateIdle()
    {
        if (playerClose())
        {
            setAttackState();
        }else {
            setPatrolState();
        }
    }

    void updatePatrol()
    {
        if (playerClose())
        {
            setAttackState();
        }

        if (Vector3.Distance(patrolPoints[nextPatrolPoint].position, transform.position) < distanceToChangePointPatrol)
        {
            MoveToNextPatrolPosition();
        }
        agent.destination = patrolPoints[nextPatrolPoint].position;
        transform.rotation = Quaternion.Lerp(transform.rotation, patrolPoints[nextPatrolPoint].rotation, Time.deltaTime * damping);
    }

    void updateAttack()
    {
        if (!playerClose()) {  setIdleState(); }
        if (hitPlayer) { setWaitingState(); }
        SetNextChasePosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LifeManager mario) && !waiting)
        {
            mario.doDamage(damageDone);
            StartCoroutine(waitToAtack());
            setWaitingState();
        }
    }

    void updateWaiting()
    {
        if (!playerClose() && !waiting) { setIdleState(); }
        else if (playerClose() && !waiting) { setAttackState(); }
    }

    void updateHit()
    {
        setIdleState();
    }

    bool playerClose()
    {
        return Vector3.Distance(player.transform.position, transform.position) < maxDistanceSeePlayer;
    }

    void MoveToNextPatrolPosition()
    {
        nextPatrolPoint++;
        if (nextPatrolPoint == patrolPoints.Count) nextPatrolPoint = 0;
    }

    void SetNextChasePosition()
    {
        agent.isStopped = false;
        agent.destination = player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * damping);
    }

    IEnumerator waitToAtack()
    {
        waiting = true;
        hitPlayer = true;
        yield return new WaitForSeconds(timeAfterHit);
        hitPlayer = false;
        waiting = false;
    }
}







//ELIMINAR AL ACABAR EL JUEGO
[CustomEditor(typeof(Enemy))]
public class customclassEditor : Editor
{

    private Enemy c;

    public void OnSceneGUI()
    {
        c = this.target as Enemy;
        Handles.color = Color.red;
        Handles.DrawWireDisc(c.transform.position + (c.transform.up * 1), c.transform.up, c.maxDistanceSeePlayer);
    }
}