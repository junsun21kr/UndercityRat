using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image itemImage; // 아이템의 이미지
    [SerializeField] Text amountText;

    public event Action<Slot> OnPointerEnterEvent;
    public event Action<Slot> OnPointerExitEvent;
    public event Action<Slot> OnRightClickEvent;
    public event Action<Slot> OnBeginDragEvent;
    public event Action<Slot> OnEndDragEvent;
    public event Action<Slot> OnDragEvent;
    public event Action<Slot> OnDropEvent;

    private Color normalColor = Color.white;
    private Color disabledColor = new Color(1,1,1,0);


    private Item _item; //획득한 아이템
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;

            if(_item == null)
            {
                itemImage.color = disabledColor;
            }
            else
            {
                itemImage.sprite = _item.itemImage;
                itemImage.color = normalColor;
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            amountText.enabled = _item != null && _item.MaximumStack > 1 && _amount > 1;
            if (amountText.enabled)
            {
                amountText.text = _amount.ToString();
            }
        }
    }

    protected virtual void OnValidate()
    {
        if(itemImage == null)
            itemImage = GetComponent<Image>();
    }

    public virtual bool CanAddStack(Item item,int amount = 1)
    {
        return Item != null && Item.ID == item.ID && Amount + amount <=item.MaximumStack;
    }

    public virtual bool CanReceiveItem(Item item)
    {
        return true;
    }

    public int itemCount; //획득한 아이템의 개수

    [SerializeField]
    private Text text_Count;
    public Image backGDIamge;

    private WeaponManager theWeaponManager;

    void Start()
    {
        theWeaponManager = FindObjectOfType<WeaponManager>();
    }  

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData !=null && eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            if(OnRightClickEvent != null)
                OnRightClickEvent(this);
        }
    }

    Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragEvent != null)
            OnBeginDragEvent(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragEvent != null)
            OnDragEvent(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragEvent != null)
            OnEndDragEvent(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (OnDropEvent != null)
            OnDropEvent(this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }

    
}
