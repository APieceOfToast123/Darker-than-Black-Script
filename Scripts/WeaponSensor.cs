using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSensor : MonoBehaviour
{

    public WeaponController wc;

    public bool isPassingPlayerOrGround;
    public bool intoGround;
    public bool intoEnemy;

    private CapsuleCollider col; 
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        wc = GetComponentInParent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(wc.throwed){
            rb = GetComponent<Rigidbody>();
        }
        if(wc.deleteRb){
            rb = GetComponent<Rigidbody>();
            if(rb != null){
                Destroy(rb);
                wc.deleteRb = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){
            isPassingPlayerOrGround = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            // Hit Enemy and Back
            isPassingPlayerOrGround = false;
            intoEnemy = true;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isPassingPlayerOrGround = true;
            intoGround = true;
            rb.isKinematic = true;
            // Stop rotating
        }
        else{
            isPassingPlayerOrGround = true;
        }
    }
}
