using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioPlayerController : MonoBehaviour, IRestartGame
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;

    [Header("JUMP")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float speedJump;
    [SerializeField] private LayerMask layerAllExceptTerrain;

    [Header("MOVEMENT")]
    [SerializeField] private KeyCode forwardKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode backKey;
    [SerializeField] private KeyCode runKey;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] private GameManager gm;


    private float verticalSpeed = -1.0f, movementSpeed;
    private bool onGround, falling, wallJumpB;
    private int nJumps = 0;
    Coroutine currentJumpCoroutine = null;


    [SerializeField] Checkpoint_classe currentCheckpoint;
    public void setCheckpoint(Checkpoint_classe checkpoint)
    {
        if(currentCheckpoint == null || currentCheckpoint.getIndex() < checkpoint.getIndex())
        {
            currentCheckpoint = checkpoint;

        }
    }

    private void Start()
    {
        gm.addRestartListener(this);
    }


    private void OnDestroy()
    {
       gm.removeRestartListener(this);
    }


    void Update()
    {
        Vector3 movement = Vector3.zero;

        Vector3 l_forward = cam.transform.forward;
        l_forward.y = 0.0f;
        l_forward.Normalize();
        
        Vector3 l_right = cam.transform.right;
        l_right.y = 0.0f;
        l_right.Normalize();

        if (Input.GetKey(forwardKey)) movement += l_forward;

        if (Input.GetKey(backKey)) movement -= l_forward;

        if (Input.GetKey(rightKey)) movement += l_right;

        if (Input.GetKey(leftKey)) movement -= l_right;

        float currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;

        if (movement.magnitude > 0)
        {
            movementSpeed = (movement.z * currentSpeed) / movement.z;
            movement.Normalize();
            transform.forward = movement.normalized;
            movement *= currentSpeed * Time.deltaTime;
        }
        else
        {
            movementSpeed = 0.0f;
        }

        if (Input.GetKeyDown(jumpKey))
        {
            if (onGround)
            {
                if (nJumps < 3 && currentJumpCoroutine != null) StopCoroutine(currentJumpCoroutine);
                jump();
                currentJumpCoroutine = StartCoroutine(resetNumberJumps());
            }else if (nearWall())
            {
                wallJump();
            }


        }
        
        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += verticalSpeed * Time.deltaTime;
        if (wallJumpB) {
            transform.forward = (transform.forward * -1) + new Vector3(0, 0, verticalSpeed * Time.deltaTime);
            wallJumpB = false;

        }




        CollisionFlags cf = controller.Move(movement);
        if((cf & CollisionFlags.Below) != 0)
        {
            onGround = true;
            falling = false;
            wallJumpB = false;
           
            verticalSpeed = -1.0f;
        }
        else
        {
            if ((cf & CollisionFlags.Above) != 0 && verticalSpeed > 0) verticalSpeed = 0.0f;
            onGround = false;
        }

        if (verticalSpeed < 0.0f) falling = true;

        if (Input.GetMouseButtonDown(0)) doPunch();

        animator.SetBool("onGround", onGround);
        animator.SetBool("falling", falling);
        animator.SetFloat("Speed", movementSpeed);

        if (nJumps >= 3) nJumps = 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out EnemyHealth enemy) && verticalSpeed < 0 && !onGround) {
            enemy.doDamage(1000); 
        }
    }


    private void wallJump()
    {
        animator.SetTrigger("wallJump");
        verticalSpeed = speedJump + nJumps;
        wallJumpB = true;
    }
    private void jump()
    {
        verticalSpeed = speedJump+nJumps;
        animator.SetTrigger("jump");
        animator.SetInteger("nJump", nJumps);
        nJumps++;
    }
    public bool nearWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f, layerAllExceptTerrain))
        {
            Debug.DrawRay(transform.position, transform.forward * 1000, Color.red);
            return true;
        }
        return false;
    }
    IEnumerator resetNumberJumps()
    {
        yield return new WaitForSeconds(2f);
        nJumps = 0;
    }

    void IRestartGame.RestartGame()
    {
        /*audioManager.Play("theme");*/
        GetComponent<CharacterController>().enabled = false;
        transform.position = currentCheckpoint.getCheckpointTransform().position;
        transform.rotation = currentCheckpoint.getCheckpointTransform().rotation;
        GetComponent<CharacterController>().enabled = true;
    }

    public void Die()
    {
        animator.SetTrigger("death");
        GetComponent<CharacterController>().enabled = false;
    }
    void doPunch()
    {
        animator.SetTrigger("punch");
    }
}
