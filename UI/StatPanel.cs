using UnityEngine;
using Kryz.CharactorStats;

public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    private CharactorStat[] stats;

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        UpdateStatNames();
    }

    public void SetStats(params CharactorStat[] charStats)
    {
        stats = charStats;

        if (stats.Length > statDisplays.Length)
        {
            print("스탯창이 부족합니다");
            return;
        }

        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < stats.Length);
        }
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].ValueText.text = stats[i].Value.ToString();
        }
        SetGMStat();
    }

    private void SetGMStat()
    {
        GameManager.Health = (int)stats[0].Value;
        GameManager.Armor = (int)stats[1].Value;
        GameManager.Stamina = (int)stats[2].Value;
    }

    public void UpdateStatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].NameText.text = statNames[i];
        }
    }
}
