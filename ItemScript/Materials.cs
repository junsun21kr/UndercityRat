using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Materials : MonoBehaviour
{
    public enum MaterialsName { Acid,Adhesive,Aluminum,Antiseptic,Ballistic_Fiber,Bone,Ceramic,Circuitry,Cloth,Concrete,Copper,Cork,Crystal,Fertilizer,Fiber_Optics,Fiberglass,Gears,Glass,Gold,Lead,Leather,Nuclear_Material,Oil,Plastic,Rubber,Screw,Silver,Spring,Diamond, AIchip, Steel,Wood}
    [SerializeField]
    private GameObject[] MaterialSlot;
    [SerializeField]
    public Text[] MaterialCount;

    private int[] indexInt = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
    private int[] sortInt = new int[32];

    [HideInInspector]
    public float[] materialDropPercent = new float[] { 2, 2, 5, 2, 2, 2, 10, 10, 2, 10, 10, 5, 5, 2, 5, 2, 5, 5, 5, 2, 5, 10, 2, 2, 10, 10, 5, 2, 2, 30, 30 };

    private void OnValidate()
    {
        SortingMaterials();
    }

    public void UnBoxing()
    {
        int unBoxingItem = GameManager.instance.Choose(materialDropPercent);
        GameManager.CurrentMaterials[unBoxingItem]++;
        MaterialCount[unBoxingItem].text = GameManager.CurrentMaterials[unBoxingItem].ToString();
    }

    public void SortingMaterials()
    {
        for (int i = 0; i < MaterialCount.Length; i++)
        {
            MaterialCount[i].text = GameManager.CurrentMaterials[i].ToString();
        }
        SortingText();
        for (int i = 0; i < MaterialSlot.Length; i++)
        {
            MaterialSlot[indexInt[i]].transform.SetSiblingIndex(i);
        }
    }

    private void SortingText()
    {
        for (int i = 0; i < sortInt.Length; i++)
        {
            sortInt[i] = int.Parse(MaterialCount[indexInt[i]].text);
        }
        int temp;
        int temp2;
        for (int i = 0; i < MaterialSlot.Length-1; i++)
        {
            for (int j = 0; j < MaterialSlot.Length - 1 - i; j++)
            {
                if (sortInt[j] < sortInt[j+1])
                {
                    temp = indexInt[j];
                    indexInt[j] = indexInt[j + 1];
                    indexInt[j + 1] = temp;
                    temp2 = sortInt[j];
                    sortInt[j] = sortInt[j + 1];
                    sortInt[j + 1] = temp2;
                }
            }
        }
    }
}
