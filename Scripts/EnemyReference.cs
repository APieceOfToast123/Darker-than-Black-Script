using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyReference : MonoBehaviour
{
    
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator anim;

    public float pathUpdateDelay = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    // public void DestroyNavMesh(){
    //     if (navMeshAgent != null) {
    //         Destroy(navMeshAgent);
    //         print("destroy");
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
