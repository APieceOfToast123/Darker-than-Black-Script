using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public IUserInput pi;
    public float horizontalSpeed = 90.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;
    public Image lockDot;
    public bool lockState;
    public bool isAI = false;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;
    [HideInInspector]
    private GameObject model;
    private new GameObject camera;
    private Vector3 cameraDampVelocity;
    [SerializeField]
    private LockTarget lockTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20.0f;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;

        if(!isAI){
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        lockState = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            //Substract    horizontalSpeed    in   joystick
            playerHandle.transform.Rotate (Vector3.up, pi.Jright * ( horizontalSpeed) * Time.fixedDeltaTime);
            tempEulerx -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 30);
            cameraHandle.transform.localEulerAngles = new Vector3 ( tempEulerx, 0, 0 );
            model.transform.eulerAngles = tempModelEuler;
        }
        else{
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
            // lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position);            
        }

        if(!isAI){
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
            camera.transform.LookAt(cameraHandle.transform);
        }
        

    }

    void Update(){
        if(lockTarget != null){
            if (!isAI){
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0,lockTarget.halfHeight,0));
            }
            
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 13.0f){
                LockProcessA(null, false, false, isAI);
            }
        }
    }

    private void LockProcessA(LockTarget _lockTarget, bool _lockDotEnable, bool _lockState, bool _isAI){
        lockTarget = _lockTarget;
        if (!_isAI)
        {
            lockDot.enabled = _lockDotEnable;
        }
        lockState = _lockState;
    }

    public void LockUnlock(){
        
            //try to lock 
            Vector3 boxCenter =  model.transform.position + new Vector3 (0, 1, 0) + model.transform.forward * 5.0f;
            Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5.0f), model.transform.rotation, LayerMask.GetMask(isAI?"Player":"Enemy"));

            if (cols.Length == 0) {
                LockProcessA(null, false, false, isAI);
            }
            else{
                foreach(var col in cols){
                    if (lockTarget != null && lockTarget.obj == col.gameObject){
                        LockProcessA(null, false, false, isAI);
                        break;
                    }
                    // print("locked: " + col.name);
                    LockProcessA(new LockTarget(col.gameObject, col.bounds.extents.y), true, true, isAI);
                    break;
                }    
            }
            
    }

    private class LockTarget{
        public GameObject obj;
        public float halfHeight;
            
        public LockTarget(GameObject _obj, float _halfHeight){
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}

