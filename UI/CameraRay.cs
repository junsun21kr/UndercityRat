using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    LayerMask layerMask = LayerMask.NameToLayer("Building");

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("버튼다운");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out hit))
            {
                if (hit.transform.gameObject.layer == 1 << 5)
                    return;
                if (hit.transform.CompareTag("Building"))
                {
                    hit.transform.GetComponent<BuildingPrefab>().ClickMouseDown();
                }
            }
        }
    }
}
