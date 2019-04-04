using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{

    private BuildingPrefab theBuildingPrefab;

    [SerializeField]
    private CraftingRecipe[] craftingRecipes;

    [SerializeField]
    private MaterialTooltip materialTooltip;

    [SerializeField]
    private Materials materials;

    [SerializeField]
    private Text buildingName;

    public void GetBuildingPrefab(BuildingPrefab buildingPrefab)
    {
        theBuildingPrefab = buildingPrefab;
        buildingName.text = theBuildingPrefab.building.ToString();
    }

    public void DestroyClick()
    {
        theBuildingPrefab.building = BuildingPrefab.BuildingName.Empty;
        theBuildingPrefab.BuildSlotUpdate();        
        theBuildingPrefab.ExitHighlight();
        SoundManager.instance.PlaySE("Destroy");
        BuildingPrefab.PopupOn = false;
        gameObject.SetActive(false);
    }

    public void ForgeLv1Click()
    {
        if (craftingRecipes[0].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[0].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[0].materialsAmount[i].MaterialsName] -= craftingRecipes[0].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.ForgeLv1;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.ForgePoint++;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
        
    }
    public void ForgeLv2Click()
    {
        if (craftingRecipes[1].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[1].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[1].materialsAmount[i].MaterialsName] -= craftingRecipes[1].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.ForgeLv2;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.ForgePoint += 2;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }
    public void ForgeLv3Click()
    {
        if (craftingRecipes[2].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[2].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[2].materialsAmount[i].MaterialsName] -= craftingRecipes[2].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.ForgeLv3;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.ForgePoint += 3;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void HospitalLv1Click()
    {
        if (craftingRecipes[3].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[3].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[3].materialsAmount[i].MaterialsName] -= craftingRecipes[3].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.HospitalLv1;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.HospitalPoint += 1;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void HospitalLv2Click()
    {
        if (craftingRecipes[4].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[4].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[4].materialsAmount[i].MaterialsName] -= craftingRecipes[4].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.HospitalLv2;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.HospitalPoint += 2;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }
    public void HospitalLv3Click()
    {
        if (craftingRecipes[5].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[5].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[5].materialsAmount[i].MaterialsName] -= craftingRecipes[5].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.HospitalLv3;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.HospitalPoint += 3;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void LabLv1Click()
    {
        if (craftingRecipes[6].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[6].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[6].materialsAmount[i].MaterialsName] -= craftingRecipes[6].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.LabLv1;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.LabPoint += 1;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void LabLv2Click()
    {
        if (craftingRecipes[7].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[7].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[7].materialsAmount[i].MaterialsName] -= craftingRecipes[7].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.LabLv2;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.LabPoint += 2;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void LabLv3Click()
    {
        if (craftingRecipes[8].CanCraft(materials))
        {
            for (int i = 0; i < craftingRecipes[8].materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)craftingRecipes[8].materialsAmount[i].MaterialsName] -= craftingRecipes[8].materialsAmount[i].Amount;
            }
            theBuildingPrefab.building = BuildingPrefab.BuildingName.LabLv3;
            theBuildingPrefab.BuildSlotUpdate();
            theBuildingPrefab.ExitHighlight();
            GameManager.LabPoint += 3;
            SoundManager.instance.PlaySE("RepairMetal");
            BuildingPrefab.PopupOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager.instance.PlaySE("UILethal2");
        }
            
    }

    public void ExitbuttonClick()
    {
        theBuildingPrefab.ExitHighlight();
        BuildingPrefab.PopupOn = false;
        gameObject.SetActive(false);
    }

    public void OnHoverButton_1()
    {
        if (craftingRecipes[0] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[0]);
        }
    }

    public void OnHoverButton_2()
    {
        if (craftingRecipes[1] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[1]);
        }
    }

    public void OnHoverButton_3()
    {
        if (craftingRecipes[2] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[2]);
        }
    }

    public void OnHoverButton_4()
    {
        if (craftingRecipes[3] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[3]);
        }
    }

    public void OnHoverButton_5()
    {
        if (craftingRecipes[4] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[4]);
        }
    }

    public void OnHoverButton_6()
    {
        if (craftingRecipes[5] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[5]);
        }
    }

    public void OnHoverButton_7()
    {
        if (craftingRecipes[6] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[6]);
        }
    }

    public void OnHoverButton_8()
    {
        if (craftingRecipes[7] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[7]);
        }
    }

    public void OnHoverButton_9()
    {
        if (craftingRecipes[8] != null)
        {
            materialTooltip.ShowTooltip(craftingRecipes[8]);
        }
    }

    public void ExitButton()
    {
        materialTooltip.HideTooltip();
    }
}
