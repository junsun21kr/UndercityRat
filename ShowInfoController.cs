using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoController : MonoBehaviour
{
    private bool pickupActivated = false; // 습득 가능한지 여부

    private RaycastHit hitInfo; // 충돌체 정보

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Transform cameraLookTarget;


    //필요한 컴포넌트
    [SerializeField]
    private Text InfoText;

    //레이를 쏠 카메라위치
    [SerializeField]
    private Camera cam;
    //플레이어의 위치
    private Transform playerTr;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        TryAction();
        CheckItem();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                print(hitInfo.transform.GetComponent<PickupItem>().item.itemName + " 획득 ");
                if (Random.Range(0, 1) == 1)
                    SoundManager.instance.PlaySE("PickItem1");
                else
                    SoundManager.instance.PlaySE("PickItem2");
                switch (hitInfo.transform.GetComponent<PickupItem>().BoxBook)
                {
                    case 0:
                        GameManager.normalBox++;
                        break;
                    case 1:
                        GameManager.RareBox++;
                        break;
                    case 2:
                        GameManager.HeroBox++;
                        break;
                    case 3:
                        GameManager.UniqueBox++;
                        break;
                    case 4:
                        GameManager.normalBook++;
                        break;                        
                    case 5:
                        GameManager.RareBook++;
                        break;
                    case 6:
                        GameManager.HeroBook++;
                        break;
                    case 7:
                        GameManager.UniqueBook++;
                        break;
                }
                //theInventory.AcquireItem(hitInfo.transform.GetComponent<PickupItem>().item);
                Destroy(hitInfo.transform.gameObject);
                
            }
        }
    }
        
    private void CheckItem()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(cam.WorldToScreenPoint(cameraLookTarget.position)), out hitInfo, 30f, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                if (Vector3.Distance(playerTr.position, hitInfo.transform.position) < 3f)
                {
                    ItemInfoAppear();
                }
            }
        }
        else
        {
            InfoDisappear();
        }
        
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        InfoText.enabled = true;
        InfoText.text = hitInfo.transform.GetComponent<PickupItem>().item.itemName + " 획득 " +"<color=yellow>"+" (E)"+"</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        InfoText.enabled =false;
    }
}
