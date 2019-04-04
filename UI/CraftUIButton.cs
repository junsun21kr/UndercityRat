using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUIButton : MonoBehaviour
{
    public Inventory inventory;

    public CraftingRecipe[] craftingRecipes;

    [SerializeField]
    private MaterialTooltip materialTooltip;

    public Materials materials;

    private void OnValidate()
    {
        if (materialTooltip == null)
            materialTooltip = FindObjectOfType<MaterialTooltip>();
    }

    public void OnCraftButton_1Click()
    {
        if(craftingRecipes[0] != null && inventory != null)
        {
            if (craftingRecipes[0].CanCraft(materials))
            {
                craftingRecipes[0].Craft(materials, inventory);
                SoundManager.instance.PlaySE("RepairMetal");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnCraftButton_2Click()
    {
        if (craftingRecipes[1] != null && inventory != null)
        {
            if (craftingRecipes[1].CanCraft(materials))
            {
                craftingRecipes[1].Craft(materials, inventory);
                SoundManager.instance.PlaySE("RepairMetal");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnCraftButton_3Click()
    {
        if (craftingRecipes[2] != null && inventory != null)
        {
            if (craftingRecipes[2].CanCraft(materials))
            {
                craftingRecipes[2].Craft(materials, inventory);
                SoundManager.instance.PlaySE("RepairMetal");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnCraftButton_4Click()
    {
        if (craftingRecipes[3] != null && inventory != null)
        {
            if (craftingRecipes[3].CanCraft(materials))
            {
                craftingRecipes[3].Craft(materials, inventory);
                SoundManager.instance.PlaySE("RepairMetal");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnHealkitCraft()
    {
        if (craftingRecipes[0] != null)
        {
            if (craftingRecipes[0].CanCraft(materials))
            {
                for (int i = 0; i < craftingRecipes[0].materialsAmount.Count; i++)
                {
                    GameManager.CurrentMaterials[(int)craftingRecipes[0].materialsAmount[i].MaterialsName] -= craftingRecipes[0].materialsAmount[i].Amount;
                }
                GameManager.healKit += 1;
                SoundManager.instance.PlaySE("PickItem2");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnRepairkitCraft()
    {
        if (craftingRecipes[1] != null)
        {
            if (craftingRecipes[1].CanCraft(materials))
            {
                for (int i = 0; i < craftingRecipes[1].materialsAmount.Count; i++)
                {
                    GameManager.CurrentMaterials[(int)craftingRecipes[1].materialsAmount[i].MaterialsName] -= craftingRecipes[1].materialsAmount[i].Amount;
                }
                GameManager.RepairKit += 1;
                SoundManager.instance.PlaySE("PickItem2");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
    }

    public void OnBulletCraft()
    {
        if (craftingRecipes[2] != null)
        {
            if (craftingRecipes[0].CanCraft(materials))
            {
                for (int i = 0; i < craftingRecipes[2].materialsAmount.Count; i++)
                {
                    GameManager.CurrentMaterials[(int)craftingRecipes[2].materialsAmount[i].MaterialsName] -= craftingRecipes[2].materialsAmount[i].Amount;
                }
                GameManager.CarryBullet += 30;
                SoundManager.instance.PlaySE("PickItem2");
            }
            else
            {
                SoundManager.instance.PlaySE("UILethal2");
            }
        }
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

    public void ExitButton()
    {
            materialTooltip.HideTooltip();
    }

}
