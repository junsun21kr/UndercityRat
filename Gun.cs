using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum SightType { DEFAULT,SIGHT3X,SNIPE6X,SNIPE8X}

    public string gunName;
    public float range;
    public float accuracy;
    public float fireRate;
    public float reloadTime;

    public int damage;    

    public int reloadBulletCount; //총알 재장전 계수
    public int currentBulletCount; //현재 탄창에 남은 총알
    public int maxBulletCount; //최대 소유 가능 개수
    public int carryBulletCount; //현재 소유하고 있는 총알 개수

    public float retroActionForce; //반동 세기
    public float retroActionFineSightForce; //정조준시 반동세기

    public float retroAForceY; // 수직반동세기
    public float retroAFineSightForceY; //정조준시 수직반동세기

    public float retroAForceX; //좌우반동세기

    public SightType gunSightType;

    public Animator anim;
    public ParticleSystem muzzleFlash;
    public Transform muzzlePoint;
    public AudioClip fire_Sound;
    
}
