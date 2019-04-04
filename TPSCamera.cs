using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float damping;
    [SerializeField] Vector3 finesightcameraOffset;
    Vector3 finesight6xcameraOffset = new Vector3(0.3f, 0.46f, -10.1f);

    private Vector3 currentCameraOffset;

    [SerializeField]
    private GameObject scopeOverlay;

    [SerializeField]
    private GameObject crossHair;


    private Transform cameraLookTarget;
    private Transform cameraTransTarget;
    private Transform playerTr;

    [HideInInspector]
    public float crouchOffset = 0f;

    [SerializeField]
    private Transform fineSightTarget;

    private GunController theGunController;
    
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        theGunController = playerTr.GetComponent<GunController>();
        cameraLookTarget = GameObject.Find("cameraLookTarget").transform;
        cameraTransTarget = GameObject.Find("cameraTransTarget").transform;
        transform.position = playerTr.position + new Vector3(0f, 2f, -7f);
        currentCameraOffset = cameraOffset;
    }


    void Update()
    {
        TPStargetLook();
    }

    void TPStargetLook()
    {
        Vector3 targetPosition = cameraTransTarget.position + playerTr.forward * currentCameraOffset.z +
            playerTr.up * (currentCameraOffset.y-(cameraLookTarget.position.y - playerTr.position.y)*0.0005f- crouchOffset) + playerTr.right * currentCameraOffset.x;

        Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, targetPosition, damping*Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, damping* Time.deltaTime);
    }

    public void CameraFineSight1Mode()
    {
        currentCameraOffset = finesightcameraOffset;
        Camera.main.fieldOfView = 55f;
    }

    public void CameraFineSight2Mode()
    {
        currentCameraOffset = finesightcameraOffset;
        Camera.main.fieldOfView = 45f;
    }

    public void CameraFineSightCancel()
    {
        currentCameraOffset = cameraOffset;
        Camera.main.fieldOfView = 60f;
        crossHair.SetActive(true);
        scopeOverlay.SetActive(false);
    }

    public void Camera6XScopeMode()
    {
        currentCameraOffset = finesight6xcameraOffset;
        Camera.main.fieldOfView = 30f;
        crossHair.SetActive(false);
        StartCoroutine(Onscope());

    }

    public void Camera8XScopeMode()
    {
        currentCameraOffset = finesight6xcameraOffset;
        Camera.main.fieldOfView = 21f;
        crossHair.SetActive(false);
        StartCoroutine(Onscope());
    }

    IEnumerator Onscope()
    {
        yield return new WaitForSeconds(0.15f);
        scopeOverlay.SetActive(true);
    }
}
