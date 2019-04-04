using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBox : MonoBehaviour
{

    //물체 체력
    [SerializeField]
    private int hp;

    //물체 콜라이더
    [SerializeField]
    private BoxCollider col;

    [SerializeField] private float[] dropItemPercent;
    [SerializeField] private float[] dropBookPercent;

    //부서지는 오브젝트
    /*[SerializeField]
    private GameObject go_Box;
    [SerializeField]
    private GameObject go_Debris;*/

    //필요한 사운드 이름
    [SerializeField]
    private string Box_HitSound, Box_DestroySound;
    

    public void BoxDamaged(int damage)
    {
        SoundManager.instance.PlaySE(Box_HitSound);
        hp -=damage;
        if (hp <= 0)
        {
            Destruction();
        }
    }

    void Destruction()
    {
        SoundManager.instance.PlaySE(Box_DestroySound);
        col.enabled = false;
        GameManager.instance.RandomDropItem(dropItemPercent, dropBookPercent, transform);
        Destroy(gameObject, 4f);
    }
}
