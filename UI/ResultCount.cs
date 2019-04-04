using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCount : MonoBehaviour
{
    private Materials materials;

    [SerializeField]
    private ShowLvExp ShowLvExp;

    private AudioSource audioSource;

    [SerializeField] private Text Exp;
    [SerializeField] private Text TechExp;

    [SerializeField] private Text[] text;

    [SerializeField] private Text[] materialCounts;

    [SerializeField] private int[] bookExp;

    [SerializeField]
    private float countSpeed=1f;

    void Start()
    {
        materials = GetComponentInChildren<Materials>();
        audioSource = GetComponent<AudioSource>();
        Exp.text = GameManager.Exp.ToString();
        TechExp.text = GameManager.TechExp.ToString();
        ShowBoxCountText();
        ShowMaterialCountText();
        StartCoroutine(ExpCount());
    }

    IEnumerator ExpCount()
    {
        print("EXPCount시작");
        yield return new WaitForSeconds(3.0f);
        int _count= GameManager.Exp;
        print("1초 대기끝");
        print(GameManager.currentExp);
        if(GameManager.currentExp != 0)
        {
            //카운팅 사운드 시작
            audioSource.Play();
            for (int i = 0; i < 80; i++)
            {
                _count = (int)Mathf.Lerp(_count, GameManager.Exp + GameManager.currentExp, Time.deltaTime * countSpeed);
                Exp.text = _count.ToString();
                yield return null;
            }
            GameManager.Exp += GameManager.currentExp;
            GameManager.currentExp = 0;
            Exp.text = GameManager.Exp.ToString();
            audioSource.Stop();
            //카운팅 사운드 끝
            yield return new WaitForSeconds(1.0f);
            GameManager.instance.CheckLevelUp();
        }

        StartCoroutine(TechExpCount());
    }

    IEnumerator TechExpCount()
    {
        yield return new WaitForSeconds(2.0f);
        int _count = GameManager.TechExp;
        int[] book = new int[] { GameManager.normalBook, GameManager.RareBook, GameManager.HeroBook, GameManager.UniqueBook };
        for (int i = 0; i < 4; i++)
        {
            if(book[i] != 0)
            {
                for (int j = 0; j < book[i]; j++)
                {
                    switch (i)
                    {
                        case 0:
                            GameManager.normalBook--;
                            break;
                        case 1:
                            GameManager.RareBook--;
                            break;
                        case 2:
                            GameManager.HeroBook--;
                            break;
                        case 3:
                            GameManager.UniqueBook--;
                            break;
                    }
                    ShowBoxCountText();
                    int _count1 = GameManager.TechExp;

                    //카운팅 사운드 시작
                    audioSource.Play();
                    for (int k = 0; k < 60; k++)
                    {
                        _count1 = (int)Mathf.Lerp(_count1, GameManager.TechExp + bookExp[i], Time.deltaTime * countSpeed);
                        TechExp.text = _count1.ToString();
                        yield return null;
                    }
                    GameManager.TechExp += bookExp[i];
                    TechExp.text = GameManager.TechExp.ToString();
                    audioSource.Stop();
                    //카운팅 사운드 끝
                    yield return new WaitForSeconds(0.3f);
                }
            }            
        }
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(openBox());
    }

    IEnumerator openBox()
    {
        yield return new WaitForSeconds(2.0f);
        int[] _box = new int[] { GameManager.normalBox, GameManager.RareBox, GameManager.HeroBox, GameManager.UniqueBox };
        for (int i = 0; i < 4; i++)
        {
            if (_box[i] != 0)
            {
                for (int j = 0; j < _box[i]; j++)
                {
                    print(i);
                    switch (i)
                    {
                        case 0:
                            GameManager.normalBox--;
                            materials.UnBoxing();
                            SoundManager.instance.PlaySE("PickItem1");
                            ShowMaterialCountText();
                            ShowBoxCountText();
                            break;
                        case 1:
                            GameManager.RareBox--;
                            materials.UnBoxing();
                            materials.UnBoxing();
                            SoundManager.instance.PlaySE("PickItem1");
                            ShowMaterialCountText();
                            ShowBoxCountText();
                            break;
                        case 2:
                            GameManager.HeroBox--;
                            materials.UnBoxing();
                            materials.UnBoxing();
                            materials.UnBoxing();
                            SoundManager.instance.PlaySE("PickItem2");
                            ShowMaterialCountText();
                            ShowBoxCountText();
                            break;
                        case 3:
                            GameManager.UniqueBox--;
                            materials.UnBoxing();
                            materials.UnBoxing();
                            materials.UnBoxing();
                            materials.UnBoxing();
                            materials.UnBoxing();
                            SoundManager.instance.PlaySE("PickItem1");
                            ShowMaterialCountText();
                            ShowBoxCountText();
                            break;
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        yield return new WaitForSeconds(2.0f);
        ShowLvExp.SetExpPanelUpdate();
        gameObject.SetActive(false);
    }    

    private void ShowBoxCountText()
    {
        text[0].text = GameManager.normalBox.ToString();
        print(GameManager.normalBox.ToString());
        text[1].text = GameManager.RareBox.ToString();
        text[2].text = GameManager.HeroBox.ToString();
        text[3].text = GameManager.UniqueBox.ToString();
        text[4].text = GameManager.normalBook.ToString();
        text[5].text = GameManager.RareBook.ToString();
        text[6].text = GameManager.HeroBook.ToString();
        text[7].text = GameManager.UniqueBook.ToString();
    }

    private void ShowMaterialCountText()
    {
        for (int i = 0; i < materialCounts.Length; i++)
        {
            materialCounts[i].text = GameManager.CurrentMaterials[i].ToString();
        }
    }
}
