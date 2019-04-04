using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyRagdollController enemyRagdollController;
    private Rocket[] rockets;

    [SerializeField] private float[] dropItemPercent;
    [SerializeField] private float[] dropBookPercent;

    private bool[] rocketFire = { false,false,false};

    private bool allRocketDone;

    private enum BossTpye { Raider, Zealot, Android,ETC }
    [SerializeField] BossTpye bossTpye = BossTpye.ETC;

    public int maxHP;
    public int exp;
    [HideInInspector]
    public int currentHP;
    private bool isDead=false;

    public int healKit=1;

    [SerializeField]private int DropLv=1;

    private void Start()
    {
        currentHP = maxHP;
        enemyRagdollController = GetComponent<EnemyRagdollController>();
        rockets = GameObject.FindObjectsOfType<Rocket>();
        switch (bossTpye)
        {
            case BossTpye.Raider:
                SoundManager.instance.PlaySE("RaiderBoss");
                break;
            case BossTpye.Zealot:
                SoundManager.instance.PlaySE("ZealotBoss");
                break;
            case BossTpye.Android:
                SoundManager.instance.PlaySE("EngagingTarget");
                break;
        }
    }

    public void EnemyDamaged(int _damage,Vector3 damageDic,bool isHead)
    {
        PopUpDamageController.CreateFloatingText(_damage.ToString(), transform, isHead);
        currentHP -= _damage;
        if(bossTpye == BossTpye.Android)
        {
            rocketCheck();

        }
        if(currentHP <= 0 && isDead==false)
        {
            for (int i = 0; i < DropLv; i++)
            {
                GameManager.instance.RandomDropItem(dropItemPercent, dropBookPercent, transform);
            }
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            GameManager.currentExp += exp;
            SoundManager.instance.PlaySE("HitDeath");
            isDead = true;


            enemyRagdollController.ChangeRagdoll(damageDic);
            Destroy(gameObject, 5f);
        }
    }    

    private void rocketCheck()
    {
        if (!allRocketDone)
        {
            if (currentHP < maxHP * 0.95 && rocketFire[0] == false)
            {
                rocketFire[0] = true;
                rockets[0].RocketFire();
            }
            else if (currentHP < maxHP * 0.8 && rocketFire[1] == false)
            {
                rocketFire[1] = true;
                rockets[1].RocketFire();
            }
            else if (currentHP < maxHP * 0.5 && rocketFire[2] == false)
            {
                rocketFire[2] = true;
                rockets[4].RocketFire();
                rockets[5].RocketFire();
            }
            else if (currentHP <maxHP * 0.3)
            {
                rockets[2].RocketFire();
                rockets[3].RocketFire();
                rockets[6].RocketFire();
                rockets[7].RocketFire();
                allRocketDone = true;
            }
        }
    }
}
