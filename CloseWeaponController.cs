using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeaponController : MonoBehaviour
{
    [SerializeField]
    private CloseWeapon currentCloseWeapon;

    private PlayerController thePlayerController;
    private GunController theGunController;

    public static bool isAttack = false;
    protected bool isSwing = false;

    [SerializeField]
    private float attackMoveSpeed;
    [SerializeField]
    private float runAttackMoveSpeed;
    
    private RaycastHit hitinfo;

    void Start()
    {
        thePlayerController = GetComponent<PlayerController>();
        theGunController = GetComponent<GunController>();
    }

    void Update()
    {
        TryMeleeAttack();
    }

    void TryMeleeAttack()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAttack)
        {
            if (!thePlayerController.randingWaitisRun && !thePlayerController.isVaulting)
            {
                if (thePlayerController.isRun)
                {
                    StartCoroutine(RunAttackCoroutine());
                }
                else
                {
                    if (thePlayerController.isCrouch)
                    {
                        thePlayerController.Crouch();
                    }
                    theGunController.CancelFineSight();
                    StartCoroutine(StandAttackCoroutine());
                }
            }
        }
    }

    protected IEnumerator StandAttackCoroutine()
    {
        isAttack = true;
        SelectCloseWeapon();
        ChangeApplySpeed(attackMoveSpeed);
        ShowWeapon(isAttack);

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격활성화 시점.
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA);
        ChangeApplySpeed(thePlayerController.walkSpeed);
        isAttack = false;
        ShowWeapon(isAttack);
    }

    protected IEnumerator RunAttackCoroutine()
    {
        isAttack = true;
        SelectCloseWeapon();
        ChangeApplySpeed(runAttackMoveSpeed);
        ShowWeapon(isAttack);

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격활성화 시점.
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA);
        ChangeApplySpeed(thePlayerController.walkSpeed);
        isAttack = false;
        ShowWeapon(isAttack);
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitinfo.transform.CompareTag("ItemBox"))
                {
                    hitinfo.transform.GetComponent<DropItemBox>().BoxDamaged(5);
                }
                isSwing = false;
            }
            yield return null;
        }
    }

    bool CheckObject()
    {
        if (Physics.SphereCast(transform.position, 0.7f,transform.TransformDirection(Vector3.forward), out hitinfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    void SelectCloseWeapon()
    {
        if (currentCloseWeapon.isDagger)
        {
            thePlayerController.anim.SetTrigger("DaggerAttack");
        }
        else if(currentCloseWeapon.isTwoHand)
        {
            thePlayerController.anim.SetTrigger("TwoHandAttack");
        }
        else if (currentCloseWeapon.isOneHand)
        {
            thePlayerController.anim.SetTrigger("OneHandAttack");
        }
    }

    void ChangeApplySpeed(float _moveSpeed)
    {
        thePlayerController.applySpeed = _moveSpeed;
    }

    void ShowWeapon(bool _change)
    {
        WeaponManager.currentWeapon.gameObject.SetActive(!_change);
        currentCloseWeapon.gameObject.SetActive(_change);
    }
}
