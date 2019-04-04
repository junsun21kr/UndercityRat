using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    #region singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion singleton


    public enum DropTable { Normal, Rare, Hero, Unique,None }

    public static int Health;
    public static int Armor;
    public static int Stamina;
    public static int CurrentDamage;
    public static int Damage1;
    public static int Damage2;

    public static int CarryBullet = 400;

    public bool isGamaOver = false;

    public static string SceneName;

    public static int normalBox;
    public static int RareBox;
    public static int HeroBox;
    public static int UniqueBox;

    public static int normalBook;
    public static int RareBook;
    public static int HeroBook;
    public static int UniqueBook;

    public static int LabPoint;
    public static int ForgePoint;
    public static int HospitalPoint;

    public static int healKit=3;
    public static int RepairKit;

    public static int PlayerLv=1;
    public static int currentExp=0;
    public static int Exp=0;
    public static int TechExp=0;
    public static int[] RquireExp = { 500,2000,5000,10000,18000,30000,50000,80000,150000,230000,300000};

    public static int[] BuildingSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static bool OnExplore = false;

    public static int[] CurrentMaterials= new int[32];

    [SerializeField]
    public static string gunSlot1 ="", gunSlot2="", pistolSlot="", closeWeapon="";

    public bool isPause =false;

    public void ResetBoxBook()
    {
        normalBox = RareBox = HeroBox = UniqueBox = normalBook = RareBook = HeroBook = UniqueBook = 0;
    }

    public void Death()
    {
        isGamaOver = true;
        OnExplore = false;
        ResetBoxBook();
        LoadScene("MainScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RandomDropItem(float[] dropItemPercent,float[] dropBookPercent ,Transform _transform)
    {
        DropTable dropTable = (DropTable)Choose(dropItemPercent);
        switch (dropTable)
        {
            case DropTable.Normal:
                Instantiate(Resources.Load("Prefabs/ItemBox_Normal"), _transform.position, Quaternion.identity);
                break;
            case DropTable.Rare:
                Instantiate(Resources.Load("Prefabs/ItemBox_Rare"), _transform.position, Quaternion.identity);
                break;
            case DropTable.Hero:
                Instantiate(Resources.Load("Prefabs/ItemBox_Hero"), _transform.position, Quaternion.identity);
                break;
            case DropTable.Unique:
                Instantiate(Resources.Load("Prefabs/ItemBox_Unique"), _transform.position, Quaternion.identity);
                break;
            default:
                break;
        }

        dropTable = (DropTable)Choose(dropBookPercent);
        switch (dropTable)
        {
            case DropTable.Normal:
                Instantiate(Resources.Load("Prefabs/TechBook_Normal"), _transform.position, Quaternion.AngleAxis(-90f,Vector3.forward));
                break;
            case DropTable.Rare:
                Instantiate(Resources.Load("Prefabs/TechBook_Rare"), _transform.position, Quaternion.AngleAxis(-90f, Vector3.forward));
                break;
            case DropTable.Hero:
                Instantiate(Resources.Load("Prefabs/TechBook_Hero"), _transform.position, Quaternion.AngleAxis(-90f, Vector3.forward));
                break;
            case DropTable.Unique:
                Instantiate(Resources.Load("Prefabs/TechBook_Unique"), _transform.position, Quaternion.AngleAxis(-90f, Vector3.forward));
                break;
            default:
                break;
        }
    }

    public int Choose(float[] probs)
    {
        float total = 0;

        for (int i = 0; i < probs.Length; i++)
        {
            total += probs[i];
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    public void CheckLevelUp()
    {
        if(Exp > RquireExp[PlayerLv - 1])
        {
            Exp -= RquireExp[PlayerLv - 1];

            PlayerLv++;
        }
    }
}
