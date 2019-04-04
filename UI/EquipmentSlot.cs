public class EquipmentSlot : Slot
{
    public WeaponType weaponType;
    public EquipmentItem equipmentItem1;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = weaponType.ToString() + " Slot";
    }
       
    public override bool CanReceiveItem(Item item)
    {
        if (item == null)
            return true;        

        EquipmentItem equipmentItem = item as EquipmentItem;
        equipmentItem1 = item as EquipmentItem;
        return equipmentItem != null && equipmentItem.weaponType == weaponType;
    }

}
