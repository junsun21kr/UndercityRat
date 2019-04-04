using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTraile : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;

    public void BulletTrail(Vector3 shootPos, Vector3 hitPos)
    {
        lr.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
        lr.SetPosition(0, shootPos);
        lr.SetPosition(1, hitPos);
        StartCoroutine(dissapearColor());
        
    }

    IEnumerator dissapearColor()
    {
        float alpha = 0.5f;
        while (alpha >= 0.01f)
        {
            print(alpha);
            lr.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, alpha));
            yield return null;
            alpha -= 0.005f;
        }      
    }
}
