using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    //Keyboard
    [Header("==== Key Settings ====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA= "left shift" ;
    public string keyB= "space";
    public string keyC= "f" ;
    public string keyD= "q" ;
    public string keyE= "left ctrl" ;
    public string keyF= "c" ;    

    public string keyJRight = "right";
    public string keyJLeft = "left";
    public string keyJUp = "up";
    public string keyJDown = "down";

    public MyKey myKeyUp = new MyKey();
    public MyKey myKeyDown = new MyKey();
    public MyKey myKeyLeft = new MyKey();
    public MyKey myKeyRight = new MyKey();
    public MyKey myKeyA = new MyKey();
    public MyKey myKeyB = new MyKey();
    public MyKey myKeyC = new MyKey();
    public MyKey myKeyD = new MyKey(); 
    public MyKey myKeyE = new MyKey();
    public MyKey myKeyF = new MyKey();  

    [Header("==== Mouse Setting ====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;

    public int mLeft = 0;
    public int mRight = 1;
    public int mMiddle = 2;

    public MyKey MouseLeft = new MyKey();
    public MyKey MouseRight = new MyKey();
    public MyKey MouseMiddle = new MyKey();


    // Update is called once per frame
    void Update()
    {

        myKeyUp.Tick(Input.GetKey(keyUp));
        myKeyDown.Tick(Input.GetKey(keyDown));
        myKeyLeft.Tick(Input.GetKey(keyLeft));
        myKeyRight.Tick(Input.GetKey(keyRight));
        myKeyA.Tick(Input.GetKey(keyA));
        myKeyB.Tick(Input.GetKey(keyB));
        myKeyC.Tick(Input.GetKey(keyC));
        myKeyD.Tick(Input.GetKey(keyD));
        myKeyE.Tick(Input.GetKey(keyE));
        myKeyF.Tick(Input.GetKey(keyF));
        
        MouseLeft.Tick(Input.GetMouseButton(mLeft));
        MouseRight.Tick(Input.GetMouseButton(mRight));
        MouseMiddle.Tick(Input.GetMouseButton(mMiddle));

        // print(myKeyA.IsExtending && myKeyA.OnPressed);

        if(mouseEnable == true){
            Jup = Input.GetAxis("Mouse Y") * 3.0f * mouseSensitivityY;
            Jright = Input.GetAxis("Mouse X") * 2.5f * mouseSensitivityX;
        }
        else{
            Jup = (Input.GetKey (keyJUp) ? 1.0f : 0) - (Input.GetKey (keyJDown) ? 1.0f : 0);
            Jright = (Input.GetKey (keyJRight) ? 1.0f : 0) - (Input.GetKey (keyJLeft) ? 1.0f : 0);
        }


        //Catch keyboard
        targetDup = (Input.GetKey(keyUp) ? 1.0f:0) - (Input.GetKey(keyDown) ? 1.0f:0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f:0) - (Input.GetKey(keyLeft) ? 1.0f:0);
       
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

        run = (myKeyA.IsPressing && !myKeyA.IsDelaying) || myKeyA.IsExtending;
        jump = myKeyC.OnPressed;
        roll = myKeyB.OnPressed;
        lockon = myKeyD.OnPressed;

        // ml: Right signal mr: Left signal A: attack S: Shield
        mlA = MouseLeft.OnReleased && MouseLeft.IsDelaying;
        mrA = MouseRight.OnReleased && MouseRight.IsDelaying;
        mlS = (MouseLeft.IsPressing && (!MouseLeft.IsDelaying)) || MouseLeft.IsExtending;
        mrS = (MouseRight.IsPressing && (!MouseRight.IsDelaying)) || MouseRight.IsExtending;
        mlHA = myKeyE.OnPressed && mlA;
        mrHA = myKeyE.OnPressed && mrA;
        mlCB = myKeyE.OnPressed && mlS;
        mrCB = myKeyE.OnPressed && mrS;
        accumulatorRelese = accumulatorAttack && MouseMiddle.OnReleased;

        defense = mlS || mrS;
        accumulatorAttack = ((MouseMiddle.IsPressing && !MouseMiddle.IsDelaying) || MouseMiddle.IsExtending);
        weaponReturn = mlS || mrS;
        
    }
}
