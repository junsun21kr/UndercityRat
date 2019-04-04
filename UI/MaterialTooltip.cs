using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MaterialTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemMaterialText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(CraftingRecipe craftingRecipe)
    {
        switch (craftingRecipe.Results[0].item.itemGrade)
        {
            case Item.ItemGradeType.Normal:
                ItemNameText.color = new Color(0.8f,0.8f,0.8f);
                break;
            case Item.ItemGradeType.Rare:
                ItemNameText.color = new Color(0.3f , 0.44f, 1.0f);
                break;
            case Item.ItemGradeType.Hero:
                ItemNameText.color = new Color(0.85f, 0.25f, 0.75f);
                break;
            case Item.ItemGradeType.Unique:
                ItemNameText.color = new Color(0.9f, 0.83f, 0.2f);
                break;
            default:
                break;
        }
        ItemNameText.text = craftingRecipe.Results[0].item.itemName;

        sb.Length = 0;
        for (int i = 0; i < craftingRecipe.materialsAmount.Count; i++)
        {
            AddStat(craftingRecipe.materialsAmount[i].Amount, craftingRecipe.materialsAmount[i].MaterialsName);
            
        }

        ExpShow(craftingRecipe.RequireTecEXP);

        ItemMaterialText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void ExpShow(int Techexp)
    {
        if(Techexp != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            sb.AppendLine();
            sb.Append("  ");
            sb.Append("TechExp");

            if (Techexp > 0)
                sb.Append(" : ");

            sb.Append(Techexp);
            sb.Append(" / ");
            sb.Append(GameManager.TechExp);
        }
    }

    private void AddStat(int value, Materials.MaterialsName statName)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
                       
            sb.Append("  ");
            sb.Append(statName.ToString());

            if (value > 0)
                sb.Append(" : ");

            sb.Append(value);
            sb.Append(" / ");
            sb.Append(GameManager.CurrentMaterials[(int)statName]);

        }
    }
}
