using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 무기 중복 교체 실행 방지.
    public static bool isChangeWeapon;

    //현재 무기와 현재무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    //공유자원. 클래스변수 = 정적변수.
    private int currentQuickSlot;

    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime,changeWeaponEndDelayTime;

    //필요한 컴포넌트
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] closeWeapon;

    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private PlayerController thePlayerController;
    
    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듬.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> CloseWeaponDictionary = new Dictionary<string, CloseWeapon>();
    

    void Start()
    {
        GameManager.CurrentDamage = GameManager.Damage1;
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < closeWeapon.Length; i++)
        {
            CloseWeaponDictionary.Add(closeWeapon[i].closeWeaponName, closeWeapon[i]);
        }
        Invoke("Quick1", 0.1f);
    }


    void Update()
    {
        if (!isChangeWeapon)
        {
            
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentQuickSlot!=1)
            {
                if (theGunController.isReload || thePlayerController.onHealing)
                    return;
                //1번무기 교체 실행
                currentQuickSlot = 1;
                StartCoroutine(ChangeWeaponCoroutine("GUN", GameManager.gunSlot1));
                GameManager.CurrentDamage = GameManager.Damage1;
            }else if (Input.GetKeyDown(KeyCode.Alpha2)&& currentQuickSlot!=2)
            {
                if (theGunController.isReload || thePlayerController.onHealing)
                    return;
                //2번 무기 교체 실행
                currentQuickSlot = 2;
                StartCoroutine(ChangeWeaponCoroutine("GUN", GameManager.gunSlot2));
                GameManager.CurrentDamage = GameManager.Damage2;
            }
        }
    }

    private void Quick1()
    {
        currentQuickSlot = 1;
        StartCoroutine(ChangeWeaponCoroutine("GUN", GameManager.gunSlot1));
        GameManager.CurrentDamage = GameManager.Damage1;
    }

    public IEnumerator ChangeWeaponCoroutine(string _type,string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type,_name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                break;
            case "HAND":
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if(_type == "HAND")
        {

        }
    }
}
