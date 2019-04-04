using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    //필요하면 HUD 호출, 필요없으면 HUD비활성화
    [SerializeField]
    private GameObject go_BulletHUD;

    //텍스트에 총알 개수 반영
    [SerializeField]
    private Text[] text_Bullet;

    void Update()
    {
        CheckBullet();
    }

    void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.currentBulletCount.ToString();
        if (GameManager.CarryBullet>999)
        {
            text_Bullet[1].text = "999";
        }
        else
        {
            text_Bullet[1].text = string.Format("{0:D3}", GameManager.CarryBullet);
        }
        
    }
}
