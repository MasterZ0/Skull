﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSounds : MonoBehaviour {
    public UnityEvent onFootLeft;
    public UnityEvent onFootRight;
    public void OnFootR() {
        //print("FootR");
    }
    public void OnFootL() {
        //print("FootL");
    }

    //Animator animator;
    //Transform rightHandObj;
    //Transform lookbObj;
    //private void OnAnimatorIK(int layerIndex) {
    //    bool Ik = true;
    //    if (Ik) {
    //        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
    //        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

    //        animator.SetLookAtWeight(1);
    //        animator.SetLookAtPosition(lookbObj.position);

    //    }
    //    else {
    //        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
    //        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    //        animator.SetLookAtWeight(0);

    //    }

    //}

    //public void OnAnimatorMove() {
    //    print("anim move");
    //}
}
