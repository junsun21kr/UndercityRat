using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MaterialsAmount
{
    public Materials.MaterialsName MaterialsName;
    public int Amount;
}

[Serializable]
public struct ItemAmount
{
    public Item item;
    [Range(1, 999)]
    public int Amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public List<MaterialsAmount> materialsAmount;
    public List<ItemAmount> Results;

    public int RequireTecEXP;

    public bool CanCraft(Materials materials)
    {

        for (int i = 0; i < materialsAmount.Count; i++)
        {
            if (GameManager.CurrentMaterials[(int)materialsAmount[i].MaterialsName] < materialsAmount[i].Amount || GameManager.TechExp< RequireTecEXP)
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(Materials materials,Inventory inventory)
    {
        if (CanCraft(materials))
        {
            for (int i = 0; i < materialsAmount.Count; i++)
            {
                GameManager.CurrentMaterials[(int)materialsAmount[i].MaterialsName] -= materialsAmount[i].Amount;
            }

            GameManager.TechExp -= RequireTecEXP;

            foreach (ItemAmount itemAmount in Results)
            {
                for (int i = 0; i < itemAmount.Amount; i++)
                {
                    inventory.AddItem(itemAmount.item.GetCopy());
                }
            }
        }
    }
}
