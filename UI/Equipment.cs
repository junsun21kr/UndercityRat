using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotsParent;
    public EquipmentSlot[] equipmentSlots;
    
    [SerializeField]
    private Text[] texts;
    private string[] originText = {"Gun","Sniper","Armor","Pistol","Melee"};

    public event Action<Slot> OnPointerEnterEvent;
    public event Action<Slot> OnPointerExitEvent;
    public event Action<Slot> OnRightClickEvent;
    public event Action<Slot> OnBeginDragEvent;
    public event Action<Slot> OnEndDragEvent;
    public event Action<Slot> OnDragEvent;
    public event Action<Slot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            equipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
            equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            equipmentSlots[i].OnDragEvent += OnDragEvent;
            equipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>(); 
    }

    public bool AddItem(EquipmentItem item,out EquipmentItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].weaponType == item.weaponType)
            {
                previousItem = (EquipmentItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                equipmentSlots[i].equipmentItem1 = item;
                ColorSet();
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquipmentItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            print(equipmentSlots[i].Item);
            if (equipmentSlots[i].Item.Equals(item))
            {
                equipmentSlots[i].Item = null;
                equipmentSlots[i].equipmentItem1 = null;
                ColorSet();
                return true;
            }
        }
        return false;
    }

    public void ColorSet()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].Item == null)
            {
                texts[i].text = originText[i];
                texts[i].color = Color.white;
                equipmentSlots[i].backGDIamge.color = new Color(0.6320754f, 0.6320754f, 0.6320754f, 0.2f);
            }
            else
            {
                texts[i].text = equipmentSlots[i].Item.itemName;
                switch (equipmentSlots[i].Item.itemGrade)
                {
                    case Item.ItemGradeType.Normal:
                        texts[i].color = new Color(0.5283019f, 0.5283019f, 0.5283019f);
                        equipmentSlots[i].backGDIamge.color = new Color(0.5283019f, 0.5283019f, 0.5283019f);
                        break;
                    case Item.ItemGradeType.Rare:
                        texts[i].color = new Color(0.2349591f, 0.2791369f, 0.986f);
                        equipmentSlots[i].backGDIamge.color = new Color(0.2349591f, 0.2791369f, 0.986f);
                        break;
                    case Item.ItemGradeType.Hero:
                        texts[i].color = new Color(0.4425514f, 0.08036668f, 0.8113208f);
                        equipmentSlots[i].backGDIamge.color = new Color(0.4425514f, 0.08036668f, 0.8113208f);
                        break;
                    case Item.ItemGradeType.Unique:
                        texts[i].color = new Color(0.8791288f, 0.9245283f, 0.1875223f);
                        equipmentSlots[i].backGDIamge.color = new Color(0.8791288f, 0.9245283f, 0.1875223f);
                        break;
                }
            }
        }
    }

    public void SetQuickSlot()
    {
        if(equipmentSlots[0].equipmentItem1 != null)
        {
            GameManager.gunSlot1 = equipmentSlots[0].Item.itemName;
            GameManager.Damage1 = (int)(equipmentSlots[0].equipmentItem1.AttackBonus*(1+GameManager.LabPoint*0.1f));
            
        }
        if (equipmentSlots[1].equipmentItem1 != null)
        {
            GameManager.gunSlot2 = equipmentSlots[1].Item.itemName;
            GameManager.Damage2 = (int)(equipmentSlots[1].equipmentItem1.AttackBonus * (1 + GameManager.LabPoint * 0.1f));
        }
        if(equipmentSlots[2].equipmentItem1 != null)
        {
            GameManager.Armor = (int)(equipmentSlots[2].equipmentItem1.ArmorBonus*(1+equipmentSlots[2].equipmentItem1.ArmorPercentBonus));
            GameManager.Health = 300+equipmentSlots[2].equipmentItem1.HealthBonus;
            GameManager.Stamina = (int)(1000*(1+equipmentSlots[2].equipmentItem1.StaminaPercentBonus));
        }
    }

}
