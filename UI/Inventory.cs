using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour, IItemContainer
{
    [FormerlySerializedAs("items")]
    [SerializeField] List<Item> startingItems;
    [SerializeField] Transform itemsParent;
    //슬롯들
    public Slot[] slots;

    public event Action<Slot> OnPointerEnterEvent;
    public event Action<Slot> OnPointerExitEvent;
    public event Action<Slot> OnRightClickEvent;
    public event Action<Slot> OnBeginDragEvent;
    public event Action<Slot> OnEndDragEvent;
    public event Action<Slot> OnDragEvent;
    public event Action<Slot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            slots[i].OnPointerExitEvent += OnPointerExitEvent;
            slots[i].OnRightClickEvent += OnRightClickEvent;
            slots[i].OnBeginDragEvent += OnBeginDragEvent;
            slots[i].OnEndDragEvent += OnEndDragEvent;
            slots[i].OnDragEvent += OnDragEvent;
            slots[i].OnDropEvent += OnDropEvent;
        }
        //SetStartingItems();
        ColorSet();
    }

    private void OnValidate()
    {
        if(itemsParent != null)
        {
            slots = itemsParent.GetComponentsInChildren<Slot>();
        }
        if (!Application.isPlaying)
        {
            SetStartingItems();
        }
        
    }    

    private void SetStartingItems()
    {
        Clear();
        int i = 0;
        for (; i < startingItems.Count && i<slots.Length; i++)
        {
            slots[i].Item = startingItems[i].GetCopy();
            slots[i].Amount = 1;
        }

        for (; i < slots.Length; i++)
        {
            slots[i].Item = null;
            slots[i].Amount = 0;
        }
        ColorSet();
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].Item == null || (slots[i].Item.ID==item.ID && slots[i].Amount<item.MaximumStack))
            {
                slots[i].Item = item;
                slots[i].Amount++;
                ColorSet();
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == item)
            {
                slots[i].Amount--;
                if (slots[i].Amount == 0)
                {
                    slots[i].Item = null;
                }
                ColorSet();
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Item item = slots[i].Item;
            if (item !=null && item.ID == itemID)
            {
                slots[i].Amount--;
                if (slots[i].Amount == 0)
                {
                    slots[i].Item = null;
                }                
                ColorSet();
                return item;
            }
        }
        return null;
    }

    public bool IsFull()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
            {
                return false;
            }
        }
        return true;
    }

    public void ColorSet()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
            {
                slots[i].backGDIamge.color = new Color(0f,0f,0f, 0.5019608f);
            }
            else
            {
                switch (slots[i].Item.itemGrade)
                {
                    case Item.ItemGradeType.Normal:
                        slots[i].backGDIamge.color = new Color(0.5283019f, 0.5283019f, 0.5283019f);
                        break;
                    case Item.ItemGradeType.Rare:
                        slots[i].backGDIamge.color = new Color(0.2349591f, 0.2791369f, 0.986f);
                        break;
                    case Item.ItemGradeType.Hero:
                        slots[i].backGDIamge.color = new Color(0.4425514f, 0.08036668f, 0.8113208f);
                        break;
                    case Item.ItemGradeType.Unique:
                        slots[i].backGDIamge.color = new Color(0.8791288f, 0.9245283f, 0.1875223f);
                        break;
                }
            }
        }
    }

    public bool ContainsItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == item)
            {                
                return true;
            }
        }
        return false;
    }

    public int ItemCount(string itemID)
    {
        int number = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item.ID == itemID)
            {
                number++;
            }
        }
        return number;
    }

    public void Clear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item != null && Application.isPlaying)
            {
                slots[i].Item.Destroy();
            }
            slots[i].Item = null;
            slots[i].Amount = 0;
        }
    }

    /*public void AcquireItem(Item _item, int _count=1)
    {

        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemName.Equals(_item.itemName))
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
                
            }
        }
        
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }*/
}
