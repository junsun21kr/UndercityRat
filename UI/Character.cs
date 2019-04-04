using UnityEngine;
using Kryz.CharactorStats;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public CharactorStat Health;
    public CharactorStat Armor;
    public CharactorStat Stamina;
    public CharactorStat AttackDamage;

    public Inventory inventory;
    public Equipment equipment;
    [SerializeField] StatPanel statPanel;
    [SerializeField] Tooltip tooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] DropItemArea dropItemArea;
    [SerializeField] QuestionDiag questionDiag;
    [SerializeField] ItemSaveManager itemSaveManager;

    private Slot draggedSlot;

    private void OnValidate()
    {
        if (tooltip == null)
            tooltip = FindObjectOfType<Tooltip>();
    }

    private void Awake()
    {
        statPanel.SetStats(Health, Armor, Stamina, AttackDamage);
        statPanel.UpdateStatValues();

        // Setup Events;
        //Right Click
        inventory.OnRightClickEvent += Equip;
        equipment.OnRightClickEvent += Unequip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipment.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipment.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipment.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipment.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipment.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipment.OnDropEvent += Drop;
        dropItemArea.OnDropEvent += DropItemOutsideUI;

    }

    private void Start()
    {
        itemSaveManager.LoadEquipment(this);
        itemSaveManager.LoadInventory(this);
    }

    public void SaveInven()
    {
        if (itemSaveManager != null)
        {
            itemSaveManager.SaveEquipment(this);
            itemSaveManager.SaveInventory(this);
        }
    }

    private void OnDestroy()
    {
        if (itemSaveManager != null)
        {
            itemSaveManager.SaveEquipment(this);
            itemSaveManager.SaveInventory(this);
        }
    }

    public void Equip(Slot slot)
    {
        EquipmentItem equipmentitem = slot.Item as EquipmentItem;
        if (equipmentitem != null)
        {
            Equip(equipmentitem);
        }
    }

    private void Unequip(Slot slot)
    {
        EquipmentItem equipmentitem = slot.Item as EquipmentItem;
        if (equipmentitem != null)
        {
            Unequip(equipmentitem);
        }
    }

    private void ShowTooltip(Slot slot)
    {
        EquipmentItem equipmentitem = slot.Item as EquipmentItem;
        if (equipmentitem != null)
        {
            tooltip.ShowTooltip(equipmentitem);
        }
    }

    private void HideTooltip(Slot slot)
    {
        tooltip.HideTooltip();
    }

    private void BeginDrag(Slot slot)
    {
        if (slot.Item != null)
        {
            draggedSlot = slot;
            draggableItem.sprite = slot.Item.itemImage;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(Slot slot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(Slot slot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }

    }

    private void Drop(Slot slot)
    {
        if (draggedSlot == null) return;

        /*if(can add stack of)
        {


        }
        */
        if (slot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(slot.Item))
        {
            EquipmentItem dragItem = draggedSlot.Item as EquipmentItem;
            EquipmentItem dropItem = slot.Item as EquipmentItem;

            if (draggedSlot is EquipmentItem)
            {
                if (dragItem != null) dragItem.Unequip(this);
                if (dropItem != null) dragItem.Equip(this);
            }
            if (slot is EquipmentItem)
            {
                if (dragItem != null) dragItem.Equip(this);
                if (dropItem != null) dragItem.Unequip(this);
            }
            statPanel.UpdateStatValues();

            Item draggedItem = draggedSlot.Item;
            int draggedItemAmount = draggedSlot.Amount;

            draggedSlot.Item = slot.Item;
            draggedSlot.Amount = slot.Amount;

            slot.Item = draggedItem;
            slot.Amount = draggedItemAmount;

            inventory.ColorSet();
            equipment.ColorSet();
        }
    }

    private void DropItemOutsideUI()
    {
        if (draggedSlot == null)
            return;

        questionDiag.Show();
        Slot slot = draggedSlot;
        questionDiag.OnYesEvent += () => DestroyItemInSlot(slot);

    }

    private void DestroyItemInSlot(Slot slot)
    {
        slot.backGDIamge.color = new Color(0f, 0f, 0f, 0.5019608f);
        slot.Item.Destroy();        
        slot.Item = null;
    }

    public void Equip(EquipmentItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquipmentItem previousItem;
            if (equipment.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquipmentItem item)
    {
        if (!inventory.IsFull() && equipment.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }
}
