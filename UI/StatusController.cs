using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int maxHp;
    private int currentHp;

    //스태미나
    [SerializeField]
    private int maxSp;
    private int currentSp;

    //스태미나 회복량
    [SerializeField]
    private int spIncreaseSpeed;

    //스태미나 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    //스태미나 감소여부
    private bool spUsed;

    //방어력
    [SerializeField]
    private int dp;
    private int currentDP;

    //필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;
    [SerializeField]
    private Text[] text_Point;
    public Image bloodScreen;
    public Image gameOverScreen;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    private const int HP = 0, SP = 1, DP = 2;

    void Start()
    {
        maxHp = GameManager.Health+(int)(GameManager.Health*(GameManager.HospitalPoint*0.2f));
        dp = GameManager.Armor+(int)(GameManager.Armor*(GameManager.ForgePoint*0.2f));
        maxSp = GameManager.Stamina;
        currentHp = maxHp;
        currentSp = maxSp;
        currentDP = dp;
    }

    void Update()
    {
        GaugeUpdate();
        PointTextUpdate();
        SPRechargeTime();
        SPRecovery();
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    private void SPRecovery()
    {
        if(!spUsed && currentSp < maxSp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / maxHp;
        images_Gauge[SP].fillAmount = (float)currentSp / maxSp;
        images_Gauge[DP].fillAmount = (float)currentDP / dp;
    }

    private void PointTextUpdate()
    {
        text_Point[HP].text = currentHp.ToString();
        text_Point[DP].text = currentDP.ToString();
    }

    public void IncreaseHP(int _count)
    {
        if(currentHp + _count < maxHp)
        {
            currentHp += _count;
        }
        else
        {
            currentHp = maxHp;
        }
    }

    public void DecreaseHP(int _count)
    {
        //블러드스크린 호출
        StartCoroutine(ShowBloodScreen());
        SoundManager.instance.PlaySE("CODhit");
        if (currentDP > 0)
        {
            DecreaseDP(_count);
            return;
        }
        currentHp -= _count;

        if(currentHp <= 0)
        {
            print("캐릭터의 HP가 0이 되었습니다.");
            GameManager.instance.isGamaOver = true;
            OnPlayerDie();
            StartCoroutine(BloodScreen());
        }
    }

    IEnumerator BloodScreen()
    {
        SoundManager.instance.PlaySE("HeartBeat");
        float alpha = 0.3f;
        float alpha2 = 0f;
        for (int i = 0; i < 60; i++)
        {
            bloodScreen.color = new Color(1f, 1f, 1f, alpha);
            gameOverScreen.color = new Color(1f, 1f, 1f, alpha2);
            yield return null;
            alpha += 0.01f;
            alpha2 += 0.003333333f;
        }
        for (int i = 0; i < 60; i++)
        {
            bloodScreen.color = new Color(1f, 1f, 1f, alpha);
            gameOverScreen.color = new Color(1f, 1f, 1f, alpha2);
            yield return null;
            alpha -= 0.01f;
            alpha2 += 0.003333333f;
        }
        for (int i = 0; i < 60; i++)
        {
            bloodScreen.color = new Color(1f, 1f, 1f, alpha);
            gameOverScreen.color = new Color(1f, 1f, 1f, alpha2);
            yield return null;
            alpha2 += 0.003333333f;
            alpha += 0.01f;
        }
        for (int i = 0; i < 60; i++)
        {
            bloodScreen.color = new Color(1f, 1f, 1f, alpha);
            gameOverScreen.color = new Color(1f, 1f, 1f, alpha2);
            yield return null;
            alpha2 += 0.003333333f;
            alpha -= 0.01f;
        }
        for (int i = 0; i < 60; i++)
        {
            bloodScreen.color = new Color(1f, 1f, 1f, alpha);
            gameOverScreen.color = new Color(1f, 1f, 1f, alpha2);
            yield return null;
            alpha2 += 0.003333333f;
            alpha += 0.01f;
        }
        bloodScreen.color = new Color(1f, 1f, 1f, 1f);
        gameOverScreen.color = new Color(1f, 1f, 1f, 1f);

        GameManager.instance.Death();
    }

    public void IncreaseDP(int _count)
    {
        if (currentDP + _count < dp)
        {
            currentDP += _count;
        }
        else
        {
            currentDP = dp;
        }
    }

    public void DecreaseDP(int _count)
    {
        currentDP -= _count;
        if (currentDP - _count > 0)
        {
            currentDP -= _count;
        }
        else
        {
            currentDP = 0;
        }

        //방어구 깨지는 소리 구현
        
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if(currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 1, 1, Random.Range(0.5f, 1.0f));
        for (int i = 0; i < 120; i++)
        {
            bloodScreen.color = new Color(1, 1, 1, bloodScreen.color.a - 0.01f);
            yield return null;
        }
        bloodScreen.color = new Color(1, 1, 1,0f);
    }
}
