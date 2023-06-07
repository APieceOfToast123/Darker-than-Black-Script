using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{

    public ActorController ac;

    [Header("=== Auto Generate if Null ===")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public WeaponSensor ws;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();

        GameObject model = ac.model;
        GameObject sensor = transform.Find("sensor").gameObject;

        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
    }

    void Start(){
        ws = wm.wmweapon.GetComponent<WeaponSensor>();
    }

    private E Bind<E>(GameObject go) where E : IActorManagerInterface
    {
        E tempInstance;
        tempInstance = go.GetComponent<E>();
        if (tempInstance == null)
        {
            tempInstance = go.AddComponent<E>();
        }
        tempInstance.am = this;

        return tempInstance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetIsCounterBack(bool value)
    {
        sm.isCounterBackEnable = value;
    }

    public void TryDoDamage(WeaponController targetWc)
    {
        if (sm.isCounterBackSuccess)
        {
            targetWc.wm.am.Stunned();
        }
        else if (sm.isCounterBackFailure)
        {
            // Still calculate
            HitOrDie(targetWc, false);
        }
        else if (sm.isImmortal)
        {
            // Do nothing!
            print("miss");
        }
        else if (sm.isDefense)
        {
            // Attack should be blocked
            Blocked();
        }
        else if (sm.isAttack)
        {
            HitOrDie(targetWc, false);
        }
        else
        {
            HitOrDie(targetWc, true);
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    // Calculate HP and determine Hit or Die
    public void HitOrDie(WeaponController targetWc, bool doHitAnimation)
    {
        if (sm.HP <= 0)
        {
            // Already dead.
        }
        else
        {
            sm.AddHP(-1 * targetWc.GetATK());
            print(-1 * targetWc.GetATK());
            if (sm.HP > 0)
            {
                if (doHitAnimation)
                {
                    Hit();
                }
                // do some VFX like splatter blood...
            }
            else
            {
                Die();
            }
        }
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;
        if (ac.camcon.lockState == true)
        {
            ac.camcon.LockUnlock();
        }
        ac.camcon.enabled = false;
    }
}
