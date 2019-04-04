using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealingPopUp : MonoBehaviour
{
    [SerializeField] private GameObject go_HealPopUp;
    [SerializeField] private GameObject go_RepairPopUp;

    [SerializeField] private Image healGageImage;
    [SerializeField] private Image repairGageImage;

    public void HealingPopUP()
    {
        go_HealPopUp.SetActive(true);
        StartCoroutine(FillGage(healGageImage, go_HealPopUp,6.0f));
    }

    public void RepairPopUp()
    {
        go_RepairPopUp.SetActive(true);
        StartCoroutine(FillGage(repairGageImage, go_RepairPopUp,4.0f));
    }

    public void StopPopUp()
    {
        StopAllCoroutines();
        go_HealPopUp.SetActive(false);
        go_RepairPopUp.SetActive(false);
    }

    IEnumerator FillGage(Image _GageImage,GameObject _gameObject,float fillTime)
    {
        //게이지 차는 소리 시작
        float _gageTime = 0f;
        while (_gageTime <= fillTime)
        {
            _GageImage.fillAmount = _gageTime / fillTime ;
            _gageTime += Time.deltaTime;
            yield return null;
        }
        //게이지 차는 소리끝
        _gameObject.SetActive(false);
    }
}
