using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCrossHair : MonoBehaviour
{
    public Transform cameraLookTarget;
    [SerializeField]
    private Gun currentGun;

    private Canvas canvas;
    private RectTransform rectTR;
    private RectTransform crosshairTr;

    private float gunAccuracy;
    private Vector3 randCrossHair;

    private PlayerController thePlayerController;
    private GunController theGunController;
    private Animator animator;
    [SerializeField]private Camera cam;
    
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTR = canvas.GetComponent<RectTransform>();
        crosshairTr = GetComponent<RectTransform>();
        thePlayerController = FindObjectOfType<PlayerController>();
        theGunController = FindObjectOfType<GunController>();
        animator = GetComponent<Animator>();
        //StartCoroutine(GetCamera());
    }

    IEnumerator GetCamera()
    {
        while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName("Play").isLoaded)
        {
            yield return null;
        }
        yield return null;
        cam = Camera.main;
    }

    private void Update()
    {
        CalCrossHairPos();
    }

    void CalCrossHairPos()
    {
        Vector2 ViewportPosition = cam.WorldToViewportPoint(cameraLookTarget.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        (ViewportPosition.x * rectTR.sizeDelta.x) - (rectTR.sizeDelta.x * 0.5f),
        (ViewportPosition.y * rectTR.sizeDelta.y) - (rectTR.sizeDelta.y * 0.5f));

        crosshairTr.anchoredPosition = WorldObject_ScreenPosition;
    }

    //스크린에서 레이를 쏴 사격위치를 계산
    public Vector3 CalAimPoint()
    {
        RaycastHit hit;
        //랜덤정확도
        randCrossHair = new Vector3(Random.Range(-GetAccuracy() - currentGun.accuracy, GetAccuracy() + currentGun.accuracy), Random.Range(-GetAccuracy() - currentGun.accuracy, GetAccuracy() + currentGun.accuracy), 0f);

        if (theGunController.GetFineSightMode() && (theGunController.GetcurrentGunSightType().Equals(Gun.SightType.SNIPE6X)|| theGunController.GetcurrentGunSightType().Equals(Gun.SightType.SNIPE8X)))
        {
            Vector3 cameraCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            Ray ray = Camera.main.ScreenPointToRay(cameraCenter);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            else
            {
                return cameraLookTarget.position;
            }
        }
        else
        {
            Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(cameraLookTarget.position) + randCrossHair * 30f);
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            else
            {
                return cameraLookTarget.position;
            }
        }        
    }

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void JumpAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }

    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }

    public void FineSightAnimation(bool _flag)
    {
        animator.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else
        {
            animator.SetTrigger("Idle_Fire");
        }
    }

    private float GetAccuracy()
    {
        if (thePlayerController.isWalk && !thePlayerController.isCrouch && !theGunController.GetFineSightMode())
        {
            gunAccuracy = 2f;
        }
        else if (thePlayerController.isCrouch && !theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.1f;
        }
        else if (theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.08f;
        }
        else
        {
            gunAccuracy = 0.6f;
        }
        return gunAccuracy;
    }

}
