using UnityEngine;
using UnityEngine.UI;

public class HealItemCount : MonoBehaviour
{
    [SerializeField]
    private Text healkitCount;
    [SerializeField]
    private Text repairkitCount;
    [SerializeField]
    private Text bulletCount;

    private void OnValidate()
    {
        healCountUISet();
    }

    public void healCountUISet()
    {
        healkitCount.text = string.Format("{0:D2}", GameManager.healKit);
        repairkitCount.text = string.Format("{0:D2}", GameManager.RepairKit);
        if (GameManager.CarryBullet > 999)
        {
            bulletCount.text = "999";
        }
        else
        {
            bulletCount.text = string.Format("{0:D3}", GameManager.CarryBullet);
        }
    }

    public void CreateHealKit()
    {
        if(GameManager.CurrentMaterials[3]>=1 && GameManager.CurrentMaterials[8] >= 2)
        {
            SoundManager.instance.PlaySE("PickItem2");
            GameManager.CurrentMaterials[3]--;
            GameManager.CurrentMaterials[8] -= 2;
            GameManager.healKit++;
            healCountUISet();
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
    }

    public void CreateRepairKit()
    {
        if(GameManager.CurrentMaterials[27]>=1&& GameManager.CurrentMaterials[25]>=1 && GameManager.CurrentMaterials[28] >= 2)
        {
            SoundManager.instance.PlaySE("PickItem2");
            GameManager.CurrentMaterials[27]--;
            GameManager.CurrentMaterials[25]--;
            GameManager.CurrentMaterials[28] -= 2;
            GameManager.RepairKit++;
            healCountUISet();
        }
        else
        {
            print("수리키트 재료가 부족합니다");
            SoundManager.instance.PlaySE("UILethal2");
        }
    }

    public void CreateBullet()
    {
        //추가코딩
        //재료 경험치
        //제작 사운드 추가
        SoundManager.instance.PlaySE("PickItem2");
        GameManager.CarryBullet += 100;
        healCountUISet();
    }
}
