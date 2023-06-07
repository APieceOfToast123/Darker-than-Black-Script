using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{

    public GameObject model;
    public CameraController camcon;
    public IUserInput pi;
    public ActorManager am;

    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 4.0f;
    public float rollVelocity = 2.0f;
    public float offsetMultiplier = 1.5f;

    [Space(10)]
    [Header("==== Friction Settings ====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private Animator anim;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    private Rigidbody wcrb;
    private GameObject cameraHandle;
    private SmoothMove smoothMove;
    // private float lerpTarget;
    private Vector3 deltaPos;

    // private bool rLongSignal;
    // private bool rShortSignal;
    // private bool lLongSignal;
    // private bool lShortSignal;

    [SerializeField]
    public bool rightIsShield = false;
    [SerializeField]
    public bool leftIsShield = true;

    // Start is called before the first frame update
    void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        am = GetComponent<ActorManager>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        cameraHandle = GameObject.Find("cameraHandle");
    }

    // Update is called once per frame
    void Update()
    {

        if (pi.lockon)
        {
            camcon.LockUnlock();
        }
        if (camcon.lockState == false)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.5f));
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDVec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward", localDVec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDVec.x * ((pi.run) ? 2.0f : 1.0f));
        }

        // anim.SetBool("defense", pi.defense);

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if (pi.roll || rigid.velocity.magnitude > 7.0f)//roll or jab?
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }
        // Light Attack (no shield + bumper)
        if ((pi.mlA || pi.mrA) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
        {
            if (pi.mlA && (!rightIsShield))
            {
                if (!am.wm.wcR.throwed)
                {
                    anim.SetBool("R0L1", false);
                    anim.SetTrigger("attack");
                }
            }
            else if (pi.mrA && (!leftIsShield))
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
        }
        // Heavy Attack (no shield + trigger pressing + bumper)
        if ((pi.mlHA || pi.mrHA) && (CheckState("ground") || CheckStateTag("attackR") || CheckStateTag("attackL")) && canAttack)
        {
            if (pi.mlHA && (!rightIsShield))
            {
                anim.SetBool("R0L1", false);
                // HeavyA
            }
            else if (pi.mrHA && (!leftIsShield))
            {
                anim.SetBool("R0L1", true);
                // HeavyA
            }
        }
        // Defense (shield + trigger pressing)
        if ((pi.mlS || pi.mrS) && (CheckState("ground") || CheckState("blocked") || CheckStateTag("defense")))
        {
            if (pi.mrS && leftIsShield)
            {
                anim.SetBool("defR0L1", true);
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defenseL"), 1);
            }
            if (pi.mlS && rightIsShield)
            {
                anim.SetBool("defR0L1", false);
                anim.SetBool("defense", pi.defense);
                anim.SetLayerWeight(anim.GetLayerIndex("defenseR"), 1);
            }
        }
        else
        {
            anim.SetBool("defense", false);
            anim.SetLayerWeight(anim.GetLayerIndex("defenseL"), 0);
            anim.SetLayerWeight(anim.GetLayerIndex("defenseR"), 0);
        }
        // Counter Back (shield + trigger pressing + bumper)
        if ((pi.mlCB || pi.mrCB) && (CheckState("ground") || CheckState("blocked") || CheckStateTag("defense")))
        {
            if (pi.mrCB && leftIsShield)
            {
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defenseL"), 0);
                anim.SetTrigger("counterBack");
            }
            if (pi.mlCB && rightIsShield)
            {
                anim.SetBool("defense", false);
                anim.SetLayerWeight(anim.GetLayerIndex("defenseR"), 0);
                anim.SetTrigger("counterBack");
            }
        }
        // Throw weapon (not shield bumper pressing)
        if (pi.accumulatorAttack && (CheckState("ground") || CheckStateTag("aiming")) && (!camcon.isAI))
        {
            if (!rightIsShield)
            {
                print("aiming");
                OnAimEnter();
                anim.SetBool("aiming", pi.accumulatorAttack);
                if (pi.accumulatorRelese)
                {
                    print("relese");
                    anim.SetBool("aiming", false);
                    anim.SetTrigger("throw");
                    OnAimExit();
                }
            }
        }
        else
        {
            anim.SetBool("aiming", false);
        }
        // Weapon Return (not shield bumper click)
        if (pi.weaponReturn && (CheckState("ground") || CheckStateTag("aiming")) && (!camcon.isAI)){
            if(am.wm.wcR.throwed && !rightIsShield){
                // trigger pull animation
                OnPullEnter();
            }
        }
        if (camcon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
            }

            if (lockPlanar == false)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        else
        {
            if (trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }

            if (lockPlanar == false)
            {
                planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }



    }

    void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool CheckState(string statename, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(statename);
    }

    public bool CheckStateTag(string tagname, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(tagname);
    }
    ///
    ///Message processing block
    ///
    public void OnJumpEnter()
    {
        pi.inputEnabled = false;
        pi.attackEnabled = false;
        lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
    }

    public void IsGround()
    {
        anim.SetBool("isGround", true);
    }


    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        pi.attackEnabled = true;
        lockPlanar = false;
        trackDirection = false;
        canAttack = true;
        col.material = frictionOne;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        pi.attackEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        pi.attackEnabled = false;
        lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, rollVelocity, rollVelocity);
    }

    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        pi.attackEnabled = false;
        lockPlanar = true;
    }

    public void OnHitEnter()
    {
        thrustVec = new Vector3(0, -0.3f, 0);
        pi.inputEnabled = false;
        planarVec = new Vector3(0, planarVec.y, 0);
    }

    public void OnDieEnter()
    {
        pi.inputEnabled = false;
        rigid.isKinematic = true;
        planarVec = new Vector3(0, planarVec.y, 0);
    }

    public void OnBlockedEnter()
    {
        pi.inputEnabled = false;
    }

    public void OnStunnedEnter()
    {
        pi.inputEnabled = false;
        planarVec = new Vector3(0, planarVec.y, 0);
    }

    public void OnCounterBackEnter()
    {
        pi.inputEnabled = false;
        planarVec = new Vector3(0, planarVec.y, 0);
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") * offsetMultiplier;
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        //lerpTarget = 1.0f;
        // anim.SetLayerWeight(1, 1.0f);
    }

    public void OnAimEnter()
    {
        pi.inputEnabled = false;
        // rigid.constraints = RigidbodyConstraints.FreezePosition;
        // model.transform.rotation = Quaternion.Euler(0f, model.transform.rotation.eulerAngles.y, 0f);
        wcrb = am.wm.wmweapon.GetComponent<Rigidbody>();
        if (wcrb == null)
        {
            wcrb = am.wm.wmweapon.AddComponent<Rigidbody>();
            wcrb.isKinematic = true;
        }

        // if (cameraHandle != null)
        // {

        // }
        // smoothMove = GetComponent<SmoothMove>();
        //     if (smoothMove == null)
        //     {
        //         smoothMove = gameObject.AddComponent<SmoothMove>();
        //     }

        //     Vector3 cameraHandleDestination = new Vector3(0f, 0.98f, 1.66f);
        //     Vector3 cameraPosOffset = new Vector3(0f, 1.56f, 0f) + new Vector3(0f, 0f, -5f);
        //     Vector3 mainCameraDestination = cameraHandleDestination + cameraPosOffset;

        //     smoothMove.MoveToDestination(Camera.main.gameObject, mainCameraDestination);
    }

    public void OnPullEnter()
    {
        wcrb.isKinematic = true;

        am.wm.wcR.isReturning = true;
        am.wm.wcR.findingOldPos = true;
    }

    public void OnAimExit()
    {
        // rigid.constraints = RigidbodyConstraints.None;
        // model.transform.rotation = Quaternion.identity;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp( anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.1f ));
    }

    public void OnAttackExit()
    {
        model.SendMessage("WeaponDisable");
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC") || CheckState("attack1hD") || CheckState("attack1hE"))
        {
            deltaPos += (deltaPos + (Vector3)_deltaPos) * 0.5f;
        }
        else if (CheckState("slash3"))
        {
            deltaPos += (deltaPos + (Vector3)_deltaPos) * 0.5f;
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
}
