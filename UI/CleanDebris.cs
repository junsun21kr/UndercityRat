using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanDebris : MonoBehaviour
{
    private BuildingPrefab theBuildingPrefab;

    public void GetBuildingPrefab(BuildingPrefab buildingPrefab)
    {
        theBuildingPrefab = buildingPrefab;
    }

    public void CleanUpDebris()
    {
        if (GameManager.TechExp >= 2000)
        {
            
            GameManager.TechExp -= 2000;
            theBuildingPrefab.building = BuildingPrefab.BuildingName.Empty;
            theBuildingPrefab.BuildSlotUpdate();
            SoundManager.instance.PlaySE("Destroy");
            theBuildingPrefab.ExitHighlight();
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
    }

    public void ExitCleanUpDebris()
    {
        BuildingPrefab.PopupOn = false;
        theBuildingPrefab.ExitHighlight();
        gameObject.SetActive(false);
    }
}
