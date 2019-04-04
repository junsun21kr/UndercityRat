using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation : MonoBehaviour
{

    private void OnEnable()
    {
        StartCoroutine(Deactive());
    }

    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(16f);
        gameObject.SetActive(false);
    }
}
