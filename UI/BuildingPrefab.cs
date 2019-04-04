using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrefab : MonoBehaviour
{
    public static bool PopupOn=false;

    [SerializeField]
    private GameObject highlightPanel;

    [SerializeField]
    private int SetNumb;

    [SerializeField]
    private GameObject[] Buildings;

    [SerializeField]
    private GameObject ButtonPanel;

    [SerializeField]
    private GameObject CleanPanel;

    public enum BuildingName { Debris,ForgeLv1,ForgeLv2, ForgeLv3,HospitalLv1, HospitalLv2, HospitalLv3,LabLv1, LabLv2, LabLv3,Empty }

    public BuildingName building = BuildingName.Debris;

    private void Awake()
    {
        building = (BuildingName)GameManager.BuildingSet[SetNumb];
        BuildSlotUpdate();
    }

    private void OnMouseEnter()
    {
        if (PopupOn)
            return;
        highlightPanel.SetActive(true);
    }

    public void OnMouseExit()
    {
        if (PopupOn)
            return;
        highlightPanel.SetActive(false);
    }

    public void ClickMouseDown()
    {
        
        if (PopupOn)
            return;
        if(building == BuildingName.Debris)
        {
            CleanPanel.SetActive(true);
            CleanPanel.transform.GetComponent<CleanDebris>().GetBuildingPrefab(GetComponent<BuildingPrefab>());
            PopupOn = true;
        }
        else
        {
            OnBuildingClick(GetComponent<BuildingPrefab>());
        }
        
    }

    public void BuildSlotUpdate()
    {
        for (int i = 0; i < Buildings.Length; i++)
        {
            Buildings[i].SetActive(false);
        }
        switch (building)
        {
            case BuildingName.Debris:
                Buildings[(int)BuildingName.Debris].SetActive(true);
                break;
            case BuildingName.ForgeLv1:
                Buildings[(int)BuildingName.ForgeLv1].SetActive(true);
                break;
            case BuildingName.ForgeLv2:
                Buildings[(int)BuildingName.ForgeLv2].SetActive(true);
                break;
            case BuildingName.ForgeLv3:
                Buildings[(int)BuildingName.ForgeLv3].SetActive(true);
                break;
            case BuildingName.HospitalLv1:
                Buildings[(int)BuildingName.HospitalLv1].SetActive(true);
                break;
            case BuildingName.HospitalLv2:
                Buildings[(int)BuildingName.HospitalLv2].SetActive(true);
                break;
            case BuildingName.HospitalLv3:
                Buildings[(int)BuildingName.HospitalLv3].SetActive(true);
                break;
            case BuildingName.LabLv1:
                Buildings[(int)BuildingName.LabLv1].SetActive(true);
                break;
            case BuildingName.LabLv2:
                Buildings[(int)BuildingName.LabLv2].SetActive(true);
                break;
            case BuildingName.LabLv3:
                Buildings[(int)BuildingName.LabLv3].SetActive(true);
                break;
            case BuildingName.Empty:
                for (int i = 0; i < Buildings.Length; i++)
                {
                    Buildings[i].SetActive(false);
                }
                break;
            default:
                break;
        }
        GameManager.BuildingSet[SetNumb] = (int)building;
    }

    public void OnBuildingClick(BuildingPrefab buildingPrefab)
    {
        PopupOn = true;
        ButtonPanel.SetActive(true);
        ButtonPanel.transform.GetComponent<BuildingPanel>().GetBuildingPrefab(buildingPrefab);
    }

    public void ExitHighlight()
    {
        highlightPanel.SetActive(false);
    }
}
