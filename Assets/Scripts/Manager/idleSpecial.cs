using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleSpecial : StateMachineBehaviour
    {
        [SerializeField] private float timeToSpecialIdle;
        private float specialIdle = 0f;
        private bool specialDone = false;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > timeToSpecialIdle && !specialDone)
            {
                specialIdle += Time.deltaTime;
                animator.SetFloat("IdleSpecial", specialIdle);
            if (specialIdle >= 1)
            {
                specialDone = true;
            }
            }
            else if (stateInfo.normalizedTime > timeToSpecialIdle+5 && specialDone)
            {
                specialIdle -= Time.deltaTime;
                animator.SetFloat("IdleSpecial", specialIdle);
                if (specialIdle <= 0)
                {
                    specialDone = false;
                    timeToSpecialIdle += stateInfo.normalizedTime;
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        specialIdle = 0f;
        specialDone = false;
        }

}