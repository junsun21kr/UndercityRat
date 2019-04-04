using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    //너클이나 맨손 구분
    public string closeWeaponName;

    //근접무기 유형
    public bool isDagger;
    public bool isTwoHand;
    public bool isOneHand;

    //공격 범위
    public float range;
    //공격력
    public int damage;
    //공격 딜레이
    public float attackDelay;
    //공격 활성화 시점
    public float attackDelayA;
    //공격 비활성화 시점
    public float attackDelayB;
}
