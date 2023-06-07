using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUserInput : MonoBehaviour
{
    [Header("==== Output signals ====")]
    public float Dup;
    public float Dright;
    public float Dmag;

    public Vector3 Dvec;

    public float Jup;
    public float Jright;


    //1.pressing signal
    public bool run;
    public bool defense;
    public bool accumulatorAttack;
    
    //2.trigger signal
    public bool jump;
    public bool roll;
    public bool lockon;

    public bool mlA;
    public bool mrA;
    public bool mlS;
    public bool mrS;
    public bool mlHA;
    public bool mrHA;
    public bool mlCB;
    public bool mrCB;
    public bool accumulatorRelese;
    public bool weaponReturn;

    //3.double trigger
    public bool attack;

    [Header("==== Others ====")]
    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    public bool inputEnabled = true;  //Flag
    public bool attackEnabled = true;

    // Square coordinates axis to elliptic coordinates axis
    protected Vector2 SquareToCircle(Vector2 input){

        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

    protected void UpdateDmagDvec(float Dup2, float Dright2){
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
    }

}