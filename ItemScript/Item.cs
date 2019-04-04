using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Item : ScriptableObject
{
    [SerializeField] string id;
    public string ID { get { return id; } }
    [Range(1,999)]
    public int MaximumStack = 1;

#if UNITY_EDITOR
    private void OnValidate()
    {        
       string path = AssetDatabase.GetAssetPath(this);
       id = AssetDatabase.AssetPathToGUID(path);
    }
#endif

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }

    public string itemName; //아이템의 이름
    public ItemType itemType; //아이템의 타입
    public ItemGradeType itemGrade; //아이템의 등급
    public Sprite itemImage; //아이템의 이미지
    //이미지는 캔버스 위에만 가능 스프라이트는 게임내에 구현가능

    public GameObject itemPrefab; // 아이템의 프리팹

    public enum ItemType
    {
        Equipment, Used, Ingredient, ETC
    }

    public enum ItemGradeType
    {
        Normal, Rare, Hero, Unique
    }

}
