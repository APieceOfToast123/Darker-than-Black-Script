using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestDirector : MonoBehaviour
{

    public PlayableDirector pd;
    public GameObject attacker;
    public GameObject victim;
    public float distance = 2f; 
    private Vector3 targetPosition; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    
        if(Input.GetKeyDown(KeyCode.H)){
            pd.time = 0;
            pd.Stop();
            pd.Evaluate();

            targetPosition = attacker.transform.position + attacker.transform.forward * distance;
            victim.transform.position = targetPosition;

            pd.Play();
            
        }
    }

}


    