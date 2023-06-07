using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wdata;
    public WeaponSensor ws;
    private GameObject weapon;
    public Transform target, curve_point;

    private Vector3 oldPos;
    private CapsuleCollider col;
    private float time = 0.0f;

    public bool throwed;
    public bool isReturning = false;
    public bool deleteRb = false;
    public bool findingOldPos = false;
    public float rotationSpeed = 4300f;
    
    // Start is called before the first frame update
    void Awake()
    {
        wdata = GetComponentInChildren<WeaponData>();
        col = GetComponentInChildren<CapsuleCollider>();
    }

    void Start(){
        if (wdata != null){
            weapon = wdata.gameObject;
            ws = BindWeaponSensor();
        }
    }

    public float GetATK(){
        return wdata.ATK;
    }

    public WeaponSensor BindWeaponSensor (){
        WeaponSensor tempWs;
        tempWs = weapon.GetComponent<WeaponSensor>();
        if(tempWs == null){
            tempWs = weapon.AddComponent<WeaponSensor>();
        }
        return tempWs;
    }

    // // Update is called once per frame
    void Update()
    {
        if(throwed){
            if (ws.intoEnemy)
            {
                col.enabled = true;
                col.isTrigger = true;
            } else if (!ws.intoGround || (ws.intoGround && isReturning)){
                col.enabled = true;
                col.isTrigger = false;
                weapon.transform.localEulerAngles += weapon.transform.forward * rotationSpeed * Time.deltaTime;
            } else if (ws.intoGround && !isReturning){
                col.enabled = false;
                col.isTrigger = false;
            } else {
                col.enabled = true;
                col.isTrigger = false;
            }
        } 
        if (isReturning) {
            if(findingOldPos){
                oldPos = weapon.transform.position;
                findingOldPos = false;
                time = 0.0f;
            }

            col.enabled = false;
            col.isTrigger = true;
            ws.intoGround = false; 
            ws.intoEnemy = false;

            if(time < 1.0f){
                weapon.transform.position = getBQCPoint(time, oldPos, curve_point.position, target.position);
                time += Time.deltaTime;
            } 
            else {
                throwed = false;
                isReturning = false;
                deleteRb = true;
                
                weapon.transform.position = target.position;
                weapon.transform.rotation = wm.whR.transform.rotation;
                weapon.transform.parent = wm.whR.transform;
            }
        }
    }

    Vector3 getBQCPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }

}
