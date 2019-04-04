using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemSlotText;
    [SerializeField] Text ItemStatsText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(EquipmentItem item)
    {
        ItemNameText.text = item.itemName;
        ItemSlotText.text = item.weaponType.ToString();

        sb.Length = 0;
        AddStat(item.AttackBonus, "Damage");
        AddStat(item.ArmorBonus, "Armor");
        AddStat(item.StaminaBonus, "SP Bonus");
        AddStat(item.HealthBonus, "HP Bonus");

        AddStat(item.AttackPercentBonus, "Damage",isPercent: true);
        AddStat(item.ArmorPercentBonus, "Armor", isPercent: true);
        AddStat(item.StaminaPercentBonus, "SP Bonus", isPercent: true);
        AddStat(item.HealthPercentBonus, "HP Bonus", isPercent: true);

        ItemStatsText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void AddStat(float value, string statName,bool isPercent =false)
    {
        if(value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+");

            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }
}
