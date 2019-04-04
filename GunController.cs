using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = true;

    //현재 장착된 총
    [SerializeField]
    private Gun currentGun;

    //연사속도 계산
    private float currentFireRat;

    private LayerMask layerMask;

    //상태 변수
    public bool isReload;
    public bool isFindSightMode = false;

    //반동 복원 위치
    private Vector3 recoilBack;
    private Vector3 retroActionRecoilBack;

    //본래 포지션 값
    [SerializeField]
    private Vector3 originPos;

    //효과음
    private AudioSource audioSource;

    //총알 충돌 정보 받아옴
    private RaycastHit hitInfo;

    //필요한 컴포넌트
    private PlayerController playerController;
    private Rigidbody myRigidbody;
    private Transform aimPoint;
    private TPSCrossHair tpsCrossHair;
    [SerializeField] private TPSCamera theTPSCamera;
    private Image crosshairImg;
    private BulletTraile bulletTraile;

    //피격 이펙트
    [SerializeField]
    private GameObject hit_effect_prefab;
    [SerializeField]
    private GameObject hit_effect_Blood;
    [SerializeField]
    private GameObject hit_effect_Wood;
    [SerializeField]
    private GameObject hit_effect_Ground;

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        myRigidbody = GetComponent<Rigidbody>();
        playerController = FindObjectOfType<PlayerController>();
        aimPoint = GameObject.Find("cameraLookTarget").transform;
        tpsCrossHair = FindObjectOfType<TPSCrossHair>();
        crosshairImg = tpsCrossHair.GetComponent<Image>();
        //recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);
        //retroActionRecoilBack = new Vector3(currentGun.findSightOriginPos.x, currentGun.findSightOriginPos.y, currentGun.retroActionFineSightForce);
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;
        layerMask = 1 << 12;
        layerMask = ~layerMask;
        bulletTraile = GetComponent<BulletTraile>();
    }

    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFindSight();
        }
        
    }

    //연사속도 재계산
    void GunFireRateCalc()
    {
        if (currentFireRat > 0)
        {
            currentFireRat -= Time.deltaTime;
        }
    }

    //발사 시도
    void TryFire()
    {
        if (Input.GetButton("Fire1"))
        {
            if (currentFireRat <= 0 && !isReload && !playerController.isRun && !CloseWeaponController.isAttack &&!playerController.isVaulting && !playerController.onHealing)
            {
                Fire();
            }
            playerController.CancelHealing();
        }
    }

    //발사 전 계산
    void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0 && !GameManager.instance.isGamaOver)
            {
                Shoot();
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }


    //재장전
    IEnumerator ReloadCoroutine()
    {
        if(GameManager.CarryBullet > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            //탄창에 남을 총알을 총 총알갯수로 넘김

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(GameManager.CarryBullet >= currentGun.reloadBulletCount)
            {
                GameManager.CarryBullet += currentGun.currentBulletCount;
                currentGun.currentBulletCount = 0;
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                GameManager.CarryBullet -= currentGun.reloadBulletCount;
            }
            else
            {
                GameManager.CarryBullet += currentGun.currentBulletCount;
                currentGun.currentBulletCount = 0;
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                GameManager.CarryBullet = 0;
            }

            isReload = false;
        }
        else
        {
            //총알 없을때 나는 틱틱 소리추가
            print("소유한 총알이 없습니다");
        }
    }

    //재장전 시도
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(!isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount && CloseWeaponController.isAttack == false && !playerController.randingWaitisRun && !playerController.isVaulting&& !GameManager.instance.isPause)
            {
                playerController.CancelHealing();
                CancelFineSight();
                playerController.RunningCancel();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //발사 후 계산
    void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRat = currentGun.fireRate;
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        
        StopAllCoroutines();
        //StartCoroutine(RetroActionCoroutine());
        if(currentGun.gunSightType == Gun.SightType.SNIPE6X || currentGun.gunSightType == Gun.SightType.SNIPE8X)
        {
            PenetrateHit();
            RetroAction();
        }
        else
        {
            Hit();
        }
    }

    private void RetroAction()
    {
        playerController.XCamRig.transform.localEulerAngles = new Vector3(transform.rotation.x + Random.Range(-currentGun.retroAForceX, currentGun.retroAForceX), transform.rotation.y - Random.Range(0, currentGun.retroAForceY), 0f);
    }

    void Hit()
    {
        Vector3 fireDiretion = (tpsCrossHair.CalAimPoint() - currentGun.muzzlePoint.position).normalized;
        
        if(Physics.Raycast(currentGun.muzzlePoint.position, fireDiretion,out hitInfo, currentGun.range, layerMask))
        {
            if (hitInfo.transform.CompareTag("Enemy"))
            {
                if(hitInfo.collider is SphereCollider)
                {
                    hitInfo.transform.GetComponent<EnemyController>().EnemyDamaged(GameManager.CurrentDamage * 2, -hitInfo.normal, true);
                }
                else
                {
                    hitInfo.transform.GetComponent<EnemyController>().EnemyDamaged(GameManager.CurrentDamage, -hitInfo.normal, false);
                }
                SoundManager.instance.PlaySE("CODhit");
                GameObject clone = Instantiate(hit_effect_Blood, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else if (hitInfo.transform.CompareTag("ItemBox"))
            {
                hitInfo.transform.GetComponent<DropItemBox>().BoxDamaged(1);
                GameObject clone = Instantiate(hit_effect_Wood, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else if (hitInfo.transform.CompareTag("Ground"))
            {
                GameObject clone = Instantiate(hit_effect_Ground, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else if (hitInfo.transform.CompareTag("Rocket"))
            {
                hitInfo.transform.GetComponent<Rocket>().HitRocket(1);
                GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else
            {
                GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(clone, 2f);
            }
            
        }
    }

    void PenetrateHit()
    {
        Vector3 fireDiretion = (tpsCrossHair.CalAimPoint() - currentGun.muzzlePoint.position).normalized;
        RaycastHit[] hits;

        hits = Physics.RaycastAll(currentGun.muzzlePoint.position, fireDiretion, currentGun.range, layerMask);

        bulletTraile.BulletTrail(currentGun.muzzlePoint.position, hits[0].point);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                if(hits[i].collider is SphereCollider)
                {
                    hits[i].transform.GetComponent<EnemyController>().EnemyDamaged(GameManager.CurrentDamage * 2, -hits[i].normal, true);
                }
                else
                {
                    hits[i].transform.GetComponent<EnemyController>().EnemyDamaged(GameManager.CurrentDamage, -hits[i].normal, false);
                }
                SoundManager.instance.PlaySE("CODhit");
                GameObject clone = Instantiate(hit_effect_Blood, hits[i].point, Quaternion.LookRotation(hits[i].normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else if (hits[i].transform.CompareTag("ItemBox"))
            {
                hits[i].transform.GetComponent<DropItemBox>().BoxDamaged(1);
                GameObject clone = Instantiate(hit_effect_Wood, hits[i].point, Quaternion.LookRotation(hits[i].normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else if (hits[i].transform.CompareTag("Ground"))
            {
                GameObject clone = Instantiate(hit_effect_Ground, hits[i].point, Quaternion.LookRotation(hits[i].normal)) as GameObject;
                Destroy(clone, 2f);
            }else if (hits[i].transform.CompareTag("Rocket"))
            {
                hits[i].transform.GetComponent<Rocket>().HitRocket(4);
                GameObject clone = Instantiate(hit_effect_prefab, hits[i].point, Quaternion.LookRotation(hits[i].normal)) as GameObject;
                Destroy(clone, 2f);
            }
            else
            {
                GameObject clone = Instantiate(hit_effect_prefab, hits[i].point, Quaternion.LookRotation(hits[i].normal)) as GameObject;
                Destroy(clone, 2f);
            }
        }

    }

    //정조준 시도
    void TryFindSight()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!isReload && !playerController.randingWaitisRun && CloseWeaponController.isAttack == false && !playerController.onHealing)
            {
                FindSight();
            }
        }
    }
    
    //정조준 취소
    public void CancelFineSight()
    {
        if (isFindSightMode)
        {
            FindSight();
        }
    }

    //정조준 로직 가동
    void FindSight()
    {
        isFindSightMode = !isFindSightMode;
        currentGun.anim.SetBool("FineSight", isFindSightMode);

        if (isFindSightMode)
        {
            switch (currentGun.gunSightType)
            {
                case Gun.SightType.DEFAULT:
                    theTPSCamera.CameraFineSight1Mode();
                    break;
                case Gun.SightType.SIGHT3X:
                    theTPSCamera.CameraFineSight2Mode();
                    break;
                case Gun.SightType.SNIPE6X:
                    theTPSCamera.Camera6XScopeMode();
                    break;
                case Gun.SightType.SNIPE8X:
                    theTPSCamera.Camera8XScopeMode();
                    break;
                default:
                    print("총의 사이트타입이 정의되지않음");
                    break;
            }
            tpsCrossHair.FineSightAnimation(isFindSightMode);
            playerController.applySpeed = 3f;
            //StopAllCoroutines();
            //StartCoroutine(FindSightActiveCoroutine());
        }
        else
        {
            theTPSCamera.CameraFineSightCancel();
            tpsCrossHair.FineSightAnimation(isFindSightMode);
            playerController.applySpeed = 5f;
            //StopAllCoroutines();
            //StartCoroutine(FindSightDeactiveCoroutine());
        }
    }
    /*
    //정조준 활성화
    IEnumerator FindSightActiveCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.findSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.findSightOriginPos, 0.2f);
            yield return null;
        }
    }

    //정조준 비활성화
    IEnumerator FindSightDeactiveCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }
    */
    /*
    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        if (!isFindSightMode)
        {
            currentGun.transform.localPosition = originPos;

            //반동 시작
            while(currentGun.transform.localPosition.z <= currentGun.retroActionForce -0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                playerController.currentCameraRotationX -= currentGun.retroAForceY;
                Camera.main.transform.localRotation = Quaternion.Euler(playerController.currentCameraRotationX, 0f, 0f);//수직반동

                Vector3 retroXForce = new Vector3(0f,Random.Range(-currentGun.retroAForceX, currentGun.retroAForceX), 0f);//좌우반동
                myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(retroXForce));
                yield return null;
            }

            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.findSightOriginPos;

            //반동 시작
            while (currentGun.transform.localPosition.z <= currentGun.retroActionFineSightForce - 0.02f)
            {
                
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                playerController.currentCameraRotationX -= currentGun.retroAFineSightForceY;
                Camera.main.transform.localRotation = Quaternion.Euler(playerController.currentCameraRotationX, 0f, 0f);//수직반동

                Vector3 retroXForce = new Vector3(0f, Random.Range(-currentGun.retroAForceX, currentGun.retroAForceX), 0f);
                myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(retroXForce));//좌우반동
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != currentGun.findSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.findSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }
    */

    //사운드 재생
    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFindSightMode;
    }

    public Gun.SightType GetcurrentGunSightType()
    {
        return currentGun.gunSightType;
    }

    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
