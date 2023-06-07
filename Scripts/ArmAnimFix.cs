using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimFix : MonoBehaviour
{
    private Animator anim;
    private ActorController ac;
    public Vector3 relaxLeft;
    public Vector3 tautLeft;
    public Vector3 relaxRight;
    public Vector3 tautRight;

    void Awake(){
        anim = GetComponent<Animator>();
        ac = GetComponentInParent<ActorController>();
    }

    void OnAnimatorIK(){

        if (ac.leftIsShield)
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            if (anim.GetBool("defense") == false)
            {
                leftLowerArm.localEulerAngles += 0.75f * relaxLeft;      
            }

            if (anim.GetBool("defense") == true){
                leftLowerArm.localEulerAngles += 0.75f * tautLeft;
            }
            
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));    
        }
        
        if (ac.rightIsShield)
        {
            Transform rightLowerArm = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            if (anim.GetBool("defense") == false)
            {
                rightLowerArm.localEulerAngles += 0.75f * relaxRight;      
            }

            if (anim.GetBool("defense") == true){
                rightLowerArm.localEulerAngles += 0.75f * tautRight;
            }
            
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightLowerArm.localEulerAngles));    
        }
                 
    }
}
