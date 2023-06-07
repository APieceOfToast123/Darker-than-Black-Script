using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    private Collider weaponColL;
    private Collider weaponColR;

    public GameObject whL;
    public GameObject whR;
    public GameObject wmweapon;

    public WeaponController wcL;
    public WeaponController wcR;

    public float throwPower = 55f;
    private Rigidbody wcrb;
    

    void Start(){
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponController (whL);
        wcR = BindWeaponController (whR);

        wmweapon = wcR.wdata.gameObject;

        weaponColL = whL.GetComponentInChildren<Collider>();
        weaponColR = whR.GetComponentInChildren<Collider>();
    }

    public WeaponController BindWeaponController(GameObject targetObj){
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if(tempWc == null){
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;

        return tempWc;
    }

    public void WeaponEnable(){
        if(am.ac.CheckStateTag("attackL")){
            weaponColL.enabled = true;
        }
        else if(am.ac.CheckStateTag("attackR")){
            weaponColR.enabled = true;
        }
        
    }
    
    public void WeaponDisable(){
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }

    public void CounterBackEnable(){
        am.SetIsCounterBack(true);
    }

    public void CounterBackDisable(){
        am.SetIsCounterBack(false);
    }

    public void OnThrowEnter(){
        wcrb = wmweapon.GetComponent<Rigidbody>();
        wcrb.isKinematic = false;

        wcR.throwed = true;
        wcR.isReturning = false;

        wmweapon.transform.parent = null;
        wcrb.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwPower, ForceMode.Impulse);
    }
}
