using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdollController : MonoBehaviour
{
    public GameObject charObj;
    public GameObject ragdollObj;

    public Rigidbody spine;

    public void ChangeRagdoll(Vector3 addForceDirection)
    {
        CopyAnimCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);

        charObj.SetActive(false);
        ragdollObj.SetActive(true);
        spine.AddForce(addForceDirection*2000f);
    }

    private void CopyAnimCharacterTransformToRagdoll(Transform origin,Transform ragdoll)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }
}
