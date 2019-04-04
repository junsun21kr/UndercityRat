using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 11f;
    [SerializeField]
    private float crouchSpeed = 2.5f;
    public float applySpeed;
    [SerializeField]
    private float range = 2f;
        
    // 상태 변수
    [HideInInspector]
    public bool isWalk = false, isRun = false, isGround = true, isCrouch = false, isAir = false, isVaulting=false, isObstacleContact=false;

    //코루틴 체크 변수
    private bool fallisRun;
    private bool Death;
    [HideInInspector]
    public bool randingWaitisRun;

    public bool onHealing;


    private CapsuleCollider capsuleCollider;
    
    private RaycastHit hit;

    //볼트,픽업 안내텍스트
    [SerializeField]private Text VaultMessage;
    //에임민감도
    private float lookSensitivity = 3f;

    //카메라 한계각도
    private float cameraRotationLimit = 55f;
    public float currentCameraRotationX;

    //필요한 컴포넌트
    public Transform XCamRig;
    public Rigidbody myRigidbody;
    [HideInInspector]
    public Animator anim;
    private GunController theGunController;
    [SerializeField]
    private TPSCamera theTPSCamera;
    private TPSCrossHair theTPSCrossHair;
    private StatusController theStatusController;
    private HealItemCount healItemCount;
    private HealingPopUp healingPopUp;
    [SerializeField]
    private GameObject healKit, repairKit;
    [SerializeField]
    private AudioSource healSound, repairSound;

    void Start()
    {
        GameManager.instance.isGamaOver = false;
        XCamRig = GameObject.Find("XCamRig").transform;
        myRigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = GetComponent<GunController>();
        theTPSCrossHair = FindObjectOfType<TPSCrossHair>();
        theStatusController = FindObjectOfType<StatusController>();
        healItemCount = FindObjectOfType<HealItemCount>();
        healingPopUp = FindObjectOfType<HealingPopUp>();

        applySpeed = walkSpeed;
        
        Cursor.visible = false;
    }

    void Update()
    {
        if(GameManager.instance.isGamaOver == false)
        {
            IsGround();
            TryJump();
            TryRun();
            TryCrouch();
            TryHealing();
            TryRepairing();
            Move();
            CheckObstacle();
        }
        else
        {
            Dead();
        }
    }

    private void Dead()
    {
        if (!Death)
        {
            anim.SetTrigger("Death");
            anim.SetInteger("DeathIdx", Random.Range(0, 2));
            Death = true;
        }

    }

    private void FixedUpdate()
    {
        CharacterRotation();
    }

    void TryHealing()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(GameManager.healKit > 0 && !onHealing && !randingWaitisRun && !isVaulting && !GameManager.instance.isPause && !CloseWeaponController.isAttack && !theGunController.isReload)
            {
                theGunController.CancelFineSight();
                StartCoroutine(HealingCoroutine());
            }
        }
    }

    void TryRepairing()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (GameManager.RepairKit > 0 && !onHealing &&!randingWaitisRun && !isVaulting && !GameManager.instance.isPause && !CloseWeaponController.isAttack && !theGunController.isReload)
            {
                theGunController.CancelFineSight();
                StartCoroutine(RepairCoroutine());
            }                
        }
    }
    //앉기 시도
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(randingWaitisRun == false)
            {
                PreventStandingInLowHeadroom();
                Crouch();
            }
        }
    }

    // 앉은상태에서 머리에 걸리는게 있으면 계속 앉은상태 유지
    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (isCrouch)
        {
            Ray crouchRay = new Ray(myRigidbody.position + Vector3.up * capsuleCollider.radius * 0.5f, Vector3.up);
            float crouchRayLength = capsuleCollider.height - capsuleCollider.radius * 0.5f;
            if (Physics.SphereCast(crouchRay, capsuleCollider.radius * 0.5f, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                isCrouch = false;
            }
        }
    }

    //앉기 동작
    public void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            anim.SetBool("Crouch", isCrouch);
            //캡슐콜라이더 조정
            capsuleCollider.center = new Vector3(0f, -0.35f, 0f);
            capsuleCollider.height = 1.3f;
            //카메라위치 조정
            theTPSCamera.crouchOffset = 0.6f;
            //크로스헤어 애니메이션
            theTPSCrossHair.CrouchingAnimation(isCrouch);
        }
        else
        {
            if (theGunController.GetFineSightMode())
            {
                applySpeed = 3f;
            }
            else
            {
                applySpeed = walkSpeed;
            }
            anim.SetBool("Crouch", isCrouch);
            //캡슐콜라이더 원위치
            capsuleCollider.center = Vector3.zero;
            capsuleCollider.height = 2.0f;
            //카메라위치 원위치
            theTPSCamera.crouchOffset = 0f;
            theTPSCrossHair.CrouchingAnimation(isCrouch);
        }
    }
    //바닥에 붙어있나 체크
    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 1.05f);
        if (isGround ==false && fallisRun ==false)
        {
                StartCoroutine(CheckFallSpeed());
        }
    }
    //떨어지는 중을 체크
    IEnumerator CheckFallSpeed()
    {
        fallisRun = true;
        theTPSCrossHair.JumpAnimation(fallisRun);
        while (true)
        {
            yield return null;
            if (isGround)
            {
                break;
            }
        }
        //떨어지는 속도가 -7보다 크면 랜딩실행
        if (myRigidbody.velocity.y <= -7f)
        {
            SoundManager.instance.PlaySE("LandLarge");
            theGunController.CancelFineSight();
            StartCoroutine(RandWaiting());
        } else if (myRigidbody.velocity.y > -7f && myRigidbody.velocity.y <= -2f)
        {
            SoundManager.instance.PlaySE("LandSmall4");
        }
        fallisRun = false;
        theTPSCrossHair.JumpAnimation(fallisRun);
    }

    //높은곳에서 착지
    IEnumerator RandWaiting()
    {
        randingWaitisRun = true;
        anim.SetTrigger("Randing");
        applySpeed = 0f;
        yield return new WaitForSeconds(1.15f);
        while (true)
        {
            applySpeed = Mathf.Lerp(applySpeed, walkSpeed, 0.1f);
            yield return null;
            if(applySpeed >= (walkSpeed - 0.1f))
            {
                break;
            }
        }
        randingWaitisRun = false;
    }

    //점프시도
    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isObstacleContact)
        {
            if(isGround && randingWaitisRun == false && theStatusController.GetCurrentSP()>0)
            {
                Ray crouchRay = new Ray(myRigidbody.position + Vector3.up * capsuleCollider.radius * 0.5f, Vector3.up);
                float crouchRayLength = capsuleCollider.height - capsuleCollider.radius * 0.5f;
                if (Physics.SphereCast(crouchRay, capsuleCollider.radius * 0.5f, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    return;
                }
                else
                {
                    theGunController.CancelFineSight();
                    Jump();
                }
            }
        }
    }
    //점프
    void Jump()
    {
        //앉은 상태에서 점프시 앉기 해제
        if (isCrouch)
        {
            Crouch();
        }
        if (isRun)
        {
            //스프린트점프
            StartCoroutine(JumpDelayForce(0.1f,4f));
        }
        else if (isWalk)
        {
            //걷기점프
            StartCoroutine(JumpDelayForce(0.1f,3f));
        }
        else
        {
            //제자리 점프
            StartCoroutine(JumpDelayForce(0.25f,4f));
        }
        theStatusController.DecreaseStamina(100);
        anim.SetTrigger("Jump");
    }

    IEnumerator JumpDelayForce(float jumpDelay,float jumpForce)
    {
        yield return new WaitForSeconds(jumpDelay);
        myRigidbody.velocity = transform.up * jumpForce;
    }

    //달리기 시도
    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(!randingWaitisRun && !theGunController.isReload && CloseWeaponController.isAttack == false && !isVaulting && theStatusController.GetCurrentSP() > 0 &&!onHealing)
            {
                Running();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetAxisRaw("Vertical")<=0f && isRun) || theStatusController.GetCurrentSP() <= 0)
        {
            if (!randingWaitisRun && CloseWeaponController.isAttack == false)
            {
                RunningCancel();
            }
        }
    }
    //달ㄹ리기
    void Running()
    {
        if (isCrouch)
        {
            Crouch();
        }
        theGunController.CancelFineSight();
        isRun = true;
        anim.SetBool("Run", isRun);
        applySpeed = Mathf.Lerp(applySpeed, runSpeed, 0.05f);
        theStatusController.DecreaseStamina(5);
        theTPSCrossHair.RunningAnimation(isRun);
    }
    //달리기 취소
    public void RunningCancel()
    {
        isRun = false;
        anim.SetBool("Run", isRun);
        applySpeed = walkSpeed;
        theTPSCrossHair.RunningAnimation(isRun);
        //anim.SetBool("Run", isRun);
    }
    //움직임 로직
    void Move()
    {
        float _moveDirX = Input.GetAxis("Horizontal");
        float _moveDirZ = Input.GetAxis("Vertical");

        Vector3 _moveDir = transform.right * _moveDirX + transform.forward * _moveDirZ;
        myRigidbody.MovePosition(transform.position + (_moveDir.normalized * applySpeed * Time.deltaTime));
        
        anim.SetFloat("Horizontal", _moveDirX);
        anim.SetFloat("Vertical", _moveDirZ);

        //무브체크
        if (!isRun && !isCrouch && isGround && !randingWaitisRun)
        {
            if (_moveDir.sqrMagnitude >= 0.01f)
            {
                isWalk = true;
                anim.SetBool("Walk", isWalk);
                theTPSCrossHair.WalkingAnimation(isWalk);
            }
            else
            {
                isWalk = false;
                anim.SetBool("Walk", isWalk);
                theTPSCrossHair.WalkingAnimation(isWalk);
            }
        }
    }    

    //좌우 캐릭터 회전 //상하 카메라 움직임
    void CharacterRotation()
    {
        XCamRig.transform.position = transform.position;
        
        float _yRotation = Input.GetAxisRaw("Mouse X");
        float _xRotation = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRotation * lookSensitivity;
        Vector3 _characterRotationY = new Vector3(0f, _yRotation , 0f)* lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        if (!randingWaitisRun && !isVaulting)
        {
            myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(_characterRotationY));
            XCamRig.transform.localEulerAngles = new Vector3(currentCameraRotationX, transform.eulerAngles.y, 0f);
        }

        anim.SetFloat("AimAngle", CheckAngle(XCamRig.transform.localEulerAngles.x));
    }

    //XCamRig의 각도를 값으로 변환
    float CheckAngle(float value)
    {
        float angle = value - 180;

        if (angle > 0)
        {
            return angle - 180;
        }

        return angle + 180;
    }

    void CheckObstacle()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1.3f, 1<<9))
        {
            VaultMessage.enabled = true;
            isObstacleContact = true;
            if (Input.GetKeyDown(KeyCode.Space) && isObstacleContact && !isVaulting)
            {
                isVaulting = true;
                switch (hit.transform.GetComponent<Obstacle_Type>().obstacleType)
                {
                    case Obstacle_Type.ObstacleType.BOX:
                        anim.SetTrigger("VaultBox");
                        StartCoroutine(VaultBox(hit.transform.GetComponent<Obstacle_Type>().offset));
                        break;
                    case Obstacle_Type.ObstacleType.WALL:
                        anim.SetTrigger("VaultWall");
                        StartCoroutine(VaultingWall(hit.transform.GetComponent<Obstacle_Type>().offset));
                        break;
                    case Obstacle_Type.ObstacleType.OVERBOX:
                        if (isRun)
                        {
                            anim.SetTrigger("JumpOverBox");
                            StartCoroutine(JumpOverBox(hit.transform.GetComponent<Obstacle_Type>().offset));
                        }
                        else
                        {
                            anim.SetTrigger("VaultBox");
                            StartCoroutine(VaultBox(hit.transform.GetComponent<Obstacle_Type>().offset));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            VaultMessage.enabled = false;
            isObstacleContact = false;
        }
    }

    IEnumerator JumpOverBox(float _offset)
    {
        transform.rotation = Quaternion.LookRotation(-hit.normal);
        applySpeed = 0f;
        StartCoroutine(JumpDelayForce(0.1f, _offset));
        capsuleCollider.height = 0.3f;
        StartCoroutine(CloseToVault(60,4f));
        yield return new WaitForSeconds(0.8f);
        capsuleCollider.height = 2f;
        yield return new WaitForSeconds(0.3f);
        applySpeed = walkSpeed;
        isVaulting = false;
    }

    IEnumerator VaultingWall(float _offset)
    {
        transform.rotation = Quaternion.LookRotation(-hit.normal);
        applySpeed = 0f;
        StartCoroutine(JumpDelayForce(0.5f, _offset));
        StartCoroutine(CloseToVault(30,2f));
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(GotoWallVaultPos());
    }

    IEnumerator CloseToVault(int _time,float _closeSpeed)
    {
        for (int i = 0; i < _time; i++)
        {
            myRigidbody.MovePosition(transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * _closeSpeed);
            yield return null;
        }
    }

    IEnumerator GotoWallVaultPos()
    {
        float _vaultUpSpeed =0.9f;
        float _vaultForwardSpeed = 3f;
        capsuleCollider.height = 0.5f;
        for (int i = 0; i < 120; i++)
        {
            myRigidbody.velocity = Vector3.up* _vaultUpSpeed;
            yield return null;
        }
        for (int i = 0; i < 20; i++)
        {
            myRigidbody.MovePosition(transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * _vaultForwardSpeed);
            yield return null;
        }
        capsuleCollider.height = 2f;
        applySpeed = walkSpeed;
        isVaulting = false;
    }

    IEnumerator VaultBox(float _offset)
    {
        transform.rotation = Quaternion.LookRotation(-hit.normal);
        applySpeed = 0f;
        StartCoroutine(JumpDelayForce(0.5f, _offset));
        StartCoroutine(CloseToVault(20,2f));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GotoBoxVaultPos());
    }

    IEnumerator GotoBoxVaultPos()
    {
        float _vaultUpSpeed = 2.5f;
        float _vaultForwardSpeed = 3f;
        for (int i = 0; i < 20; i++)
        {
            myRigidbody.velocity = Vector3.up * _vaultUpSpeed;
            yield return null;
        }
        for (int i = 0; i < 20; i++)
        {
            myRigidbody.MovePosition(transform.position + transform.TransformDirection(Vector3.forward) * Time.deltaTime * _vaultForwardSpeed);
            yield return null;
        }
        applySpeed = walkSpeed;
        isVaulting = false;
    }


    IEnumerator HealingCoroutine()
    {
            onHealing = true;
        healSound.Play();
        healingPopUp.HealingPopUP();
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        anim.SetBool("Healing", onHealing);
        healKit.SetActive(onHealing);
        yield return new WaitForSeconds(6f);
            theStatusController.IncreaseHP((int)(GameManager.Health * 0.3f));
        GameManager.healKit--;
            onHealing = false;
        anim.SetBool("Healing", onHealing);
        healKit.SetActive(onHealing);
        healItemCount.healCountUISet();
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(true);
        }
        SoundManager.instance.PlaySE("EndHeal");
    }

    IEnumerator RepairCoroutine()
    {
            onHealing = true;
        repairSound.Play();
        healingPopUp.RepairPopUp();
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        repairKit.SetActive(onHealing);
        anim.SetBool("Healing", onHealing);
        yield return new WaitForSeconds(4f);
        theStatusController.IncreaseDP((int)(GameManager.Armor * 0.5f));
        print(GameManager.Armor * 0.5f);
        GameManager.RepairKit--;
            onHealing = false;
        repairKit.SetActive(onHealing);
        anim.SetBool("Healing", onHealing);
        healItemCount.healCountUISet();
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(true);
        }
        SoundManager.instance.PlaySE("EndHeal");
    }

    public void CancelHealing()
    {
        if (onHealing)
        {
            healSound.Stop();
            repairSound.Stop();
            healingPopUp.StopPopUp();
            StopAllCoroutines();
            anim.SetBool("Healing",false);
            StartCoroutine(CancelHealingCoolTime());
            if (WeaponManager.currentWeapon != null)
            {
                WeaponManager.currentWeapon.gameObject.SetActive(true);
            }
            healKit.SetActive(false);
            repairKit.SetActive(false);
            SoundManager.instance.PlaySE("EndHeal");
        }
    }

    IEnumerator CancelHealingCoolTime()
    {
        yield return new WaitForSeconds(0.5f);

        onHealing = false;
    }
}
