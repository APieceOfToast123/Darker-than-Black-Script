using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defCol;
    private WeaponSensor ws;
    private bool isPlayer;

    void Start(){
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.5f;
        defCol.isTrigger = true;
    }

    void OnTriggerEnter(Collider col){

        if (transform.parent != null){
            bool isPlayer = (transform.parent.tag == "Player");
        }

        if(col.tag == "Weapon"){
            ws = col.GetComponent<WeaponSensor>();
            // if(ws.isPassingPlayerOrGround || isPlayer){
            //     // Do nothing!
            //     print("pass");
            //     ws.isPassingPlayerOrGround = false;
            //     return;
            // }
            // else
            // {
                WeaponController targetWc = ws.wc;
                am.TryDoDamage(targetWc);
            // }
        }
    }
}
