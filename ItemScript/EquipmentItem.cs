using UnityEngine;
using Kryz.CharactorStats;


public enum WeaponType { GUN,SNIPER, ARMOR, PISTOL, MELEE }

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/EquipmentItem")]
public class EquipmentItem : Item
{
    public int AttackBonus;
    public int HealthBonus;
    public int StaminaBonus;
    public int ArmorBonus;
    [Space]
    public float AttackPercentBonus;
    public float HealthPercentBonus;
    public float StaminaPercentBonus;
    public float ArmorPercentBonus;
    [Space]
    public WeaponType weaponType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(Character c)
    {
        if (AttackBonus != 0)
            c.AttackDamage.AddModifier(new StatModifier(AttackBonus, StatModType.Flat, this));
        if (HealthBonus != 0)
            c.Health.AddModifier(new StatModifier(HealthBonus, StatModType.Flat, this));
        if (StaminaBonus != 0)
            c.Stamina.AddModifier(new StatModifier(StaminaBonus, StatModType.Flat, this));
        if (ArmorBonus != 0)
            c.Armor.AddModifier(new StatModifier(ArmorBonus, StatModType.Flat, this));

        if (AttackPercentBonus != 0)
            c.AttackDamage.AddModifier(new StatModifier(AttackPercentBonus, StatModType.PercentMult, this));
        if (HealthPercentBonus != 0)
            c.Health.AddModifier(new StatModifier(HealthPercentBonus, StatModType.PercentMult, this));
        if (StaminaPercentBonus != 0)
            c.Stamina.AddModifier(new StatModifier(StaminaPercentBonus, StatModType.PercentMult, this));
        if (ArmorPercentBonus != 0)
            c.Armor.AddModifier(new StatModifier(ArmorPercentBonus, StatModType.PercentMult, this));

    }

    public void Unequip(Character c)
    {
        c.AttackDamage.RemoveAllModifiersFromSource(this);
        c.Health.RemoveAllModifiersFromSource(this);
        c.Stamina.RemoveAllModifiersFromSource(this);
        c.Armor.RemoveAllModifiersFromSource(this);
    }
}
