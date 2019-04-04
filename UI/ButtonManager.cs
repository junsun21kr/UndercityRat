using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject go_CraftPanel;
    [SerializeField] private GameObject go_ResultPanel;
    private bool CraftPanelOnOff;

    [SerializeField] BGM bgm;

    private void Start()
    {
        SoundManager.instance.PlayTitleBGM((int)bgm);
        if (GameManager.OnExplore)
        {
            go_ResultPanel.SetActive(true);
            GameManager.OnExplore = false;
        }
            
    }

    private void OnDestroy()
    {
        SoundManager.instance.StopBGM();
    }

    public void ExplorClick()
    {
        GameManager.instance.LoadScene("TPSTest");
    }

    public void CraftClick()
    {
        if (!CraftPanelOnOff)
        {
            go_CraftPanel.SetActive(true);
            CraftPanelOnOff = true;
        }
        else
        {
            go_CraftPanel.SetActive(false);
            CraftPanelOnOff = false;
        }
    }

    public void BTNClickSound1()
    {
        SoundManager.instance.PlaySE("UIsound1");
    }
    public void BTNClickSound2()
    {
        SoundManager.instance.PlaySE("UIsound2");
    }
    public void BTNClickSound3()
    {
        SoundManager.instance.PlaySE("UIsound3");
    }
    public void BTNClickSound4()
    {
        SoundManager.instance.PlaySE("UIsound4");
    }

    public void GOExplorSceneLaider()
    {
        GameManager.SceneName = "RaiderMain";
        SceneManager.LoadScene("SceneLoader");
    }

    public void GOExplorSceneZealot()
    {
        GameManager.SceneName = "ZealotMain";
        SceneManager.LoadScene("SceneLoader");
    }

    public void GOExplorSceneAndroid()
    {
        GameManager.SceneName = "AndroidMain";
        SceneManager.LoadScene("SceneLoader");
    }

    public void GOExplorSceneSunsetPoint()
    {
        GameManager.SceneName = "SunsetPointMain";
        SceneManager.LoadScene("SceneLoader");
    }

    public void Cheatbutton()
    {
        for (int i = 0; i < GameManager.CurrentMaterials.Length; i++)
        {
            GameManager.CurrentMaterials[i] += 100;
            GameManager.TechExp += 10000;
        }
    }
}
