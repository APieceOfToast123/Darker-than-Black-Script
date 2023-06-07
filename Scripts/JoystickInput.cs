using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IUserInput
{
    //Keyboard
    [Header("==== Joystick Settings ====")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axisJright = "axis4";
    public string axisJup = "axis5";

    public string btnA = "btn0" ;
    public string btnB = "btn1";
    public string btnC = "btn2" ;
    public string btnD = "btn3" ;
    public string btnLB= "btn4" ;
    public string btnRB= "btn5" ; 
    public string btnTR= "axis3" ;  


    public MyKey buttonA = new MyKey();
    public MyKey buttonB = new MyKey();
    public MyKey buttonC = new MyKey();
    public MyKey buttonD = new MyKey();
    public MyKey buttonLB = new MyKey();
    public MyKey buttonRB = new MyKey();
    public MyKey buttonLT = new MyKey();
    public MyKey buttonRT = new MyKey();

    // Update is called once per frame
    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonLT.Tick((Input.GetAxisRaw(btnTR) < 0) ? true : false);
        buttonRT.Tick((Input.GetAxisRaw(btnTR) > 0) ? true : false);

        Jup = Input.GetAxis (axisJup);
        Jright = Input.GetAxis (axisJright);

        //Catch keyboard
        targetDup = Input.GetAxis(axisY);
        targetDright = Input.GetAxis(axisX);
       
        //Soft close
        if(inputEnabled == false){
            targetDup = 0;
            targetDright = 0;
        }

        //Change the target velocity (SmoothDamp)
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.2f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.2f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        UpdateDmagDvec(Dup2, Dright2);

        run = (buttonA.IsPressing && !buttonA.IsDelaying) || buttonA.IsExtending;
        jump = buttonD.OnPressed;
        roll = buttonB.OnPressed;
        lockon = buttonC.OnPressed;

        // ml: Right signal mr: Left signal A: attack S: Shield
        mlA = buttonRB.OnPressed;
        mrA = buttonLB.OnPressed;
        mlS = (buttonRT.IsPressing && (!buttonRT.IsDelaying)) || buttonRT.IsExtending;
        mrS = (buttonLT.IsPressing && (!buttonLT.IsDelaying)) || buttonLT.IsExtending;
        mlHA = mlA && mlS;
        mrHA = mrA && mrS;
        mlCB = mlA && mlS;
        mrCB = mrA && mrS;
        accumulatorRelese = accumulatorAttack && (buttonLT.OnReleased || buttonRT.OnReleased);

        defense = mlS || mrS;
        accumulatorAttack = (buttonRT.IsPressing && (!buttonRT.IsDelaying)) || buttonRT.IsExtending;
        weaponReturn = buttonRT.OnPressed;
    }
}
