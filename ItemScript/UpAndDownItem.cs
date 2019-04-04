using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownItem : MonoBehaviour
{
    [SerializeField]
    private float upForce;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * upForce);
    }
}
