using UnityEngine;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour
{
    [SerializeField] private GameObject go_BaseUI;
    [SerializeField] private bool onVisualBox;
    [SerializeField] private Text[] text;

    [SerializeField]
    private bool isMain = false;
    
       
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.instance.isPause)
                CallMenu();
            else
                CloseMenu();
        }
    }

    private void CallMenu()
    {
        if(onVisualBox == true)
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
        
        GameManager.instance.isPause = true;
        Cursor.visible = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        GameManager.instance.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1f;
        if (!isMain)
            Cursor.visible = false;
    }

    public void ClickEscape()
    {
        GameManager.OnExplore = true;
        Time.timeScale = 1f;
        Debug.Log("탈출");
        GameManager.instance.LoadScene("MainScene");
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

    public void ClickSave()
    {
        Debug.Log("저장됨");
    }
}
