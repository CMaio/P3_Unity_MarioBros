using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNewBehaviour : StateMachineBehaviour
{
    [SerializeField] private float startTime = 0.3f;
    [SerializeField] private float endTime = 0.7f;
    private PunchController punchController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        punchController = animator.gameObject.GetComponentInChildren<PunchController>();
        //punchController.setPunch(true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        punchController.setPunch(stateInfo.normalizedTime > startTime && stateInfo.normalizedTime < endTime);
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    punchController.setPunch(false);
    //}

}
