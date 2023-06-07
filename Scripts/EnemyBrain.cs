using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : IUserInput
{
    public GameObject tgtmodel;
    [HideInInspector] public Transform target;
    // [HideInInspector] public Transform enemyTrans;
    public bool inRange;
    public bool find;
    private EnemyReference enemyReference;
    private ActorController ac;
    private ActorController tgtAc;
    private float attackDistance;
    public float eyeDistance;
    private float pathUpdateDeadline;
    // private float pathUpdateDeadline;
    private float currentMovementTime;

    void Awake()
    {
        target = tgtmodel.transform;
        // enemyTrans = transform.Find("BanditFist");

        enemyReference = GetComponent<EnemyReference>();
        ac = GetComponent<ActorController>();
        tgtAc = tgtmodel.GetComponent<ActorController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackDistance = enemyReference.navMeshAgent.stoppingDistance;
        eyeDistance = 4 * attackDistance;

        find = Vector3.Distance(transform.position, target.position) <= eyeDistance;
        // if (!find)
        // {
        //     // 
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){

            find = Vector3.Distance(transform.position, target.position) <= eyeDistance;
            inRange = Vector3.Distance(transform.position, target.position) <= attackDistance;

            if(find){
                
                if (inRange) {
                    LookAtTarget();
                    StartCoroutine(AttackRoutine());
                    // enemyReference.anim.SetTrigger("attack");
                    Dup = Mathf.Lerp(Dup, 0f, 0.05f * Time.deltaTime);
                } else {
                    if(ac.CheckStateTag("attackR") || ac.CheckStateTag("attackL")){
                        Dup = 0f;
                        mlA = false;
                        print("looking around");
                        enemyReference.anim.SetTrigger("lookingAround");
                    } else {
                        UpdatePath();
                        mlA = false;
                        Dup = 1.5f;
                    }
                    
                }

                if(tgtAc.CheckState("die")){
                    target = null;
                    enemyReference.anim.SetTrigger("lookingAround");
                }
            } else {
                Dup = 0f;
                // StartCoroutine(idleRoutine());
            }

            if(ac.CheckState("lookingAround") && find){
                enemyReference.anim.SetTrigger("find");
            } 
            
            
            if(ac.CheckState("die")){
                target = null;
            } 

            UpdateDmagDvec(Dup, Dright);
           
        }
    }

    private void LookAtTarget(){
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation =  Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }
    private void UpdatePath(){
        if(Time.time >= pathUpdateDeadline ){
            // print("Updating Path");
            pathUpdateDeadline = Time.time + enemyReference.pathUpdateDelay;
            enemyReference.navMeshAgent.SetDestination(target.position);
        }
    }



    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            float attackTime = Random.Range(1f, 4f);
            yield return new WaitForSeconds(attackTime);

            enemyReference.anim.SetTrigger("attack");
            yield return new WaitForSeconds(enemyReference.anim.GetCurrentAnimatorStateInfo(0).length);

            while (enemyReference.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
        }
    }

    private IEnumerator idleRoutine() {
        while (true) {
            if (inRange)
            {
                enemyReference.anim.SetTrigger("idle");

                float idleTime = Random.Range(1f, 3f);

                yield return new WaitForSeconds(idleTime);

                yield return new WaitForSeconds(enemyReference.anim.GetCurrentAnimatorStateInfo(0).length);

                while (enemyReference.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                {
                    yield return null;
                }
            }
            else
            {
                enemyReference.anim.SetTrigger("idle");
            }

        }
    }
}
