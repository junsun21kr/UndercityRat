using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName;

    private void Start()
    {
        SoundManager.instance.PlayTitleBGM(0);
    }

    public void ClickStart()
    {
        GameManager.instance.LoadScene(sceneName);
        SoundManager.instance.StopBGM();
    }

    public void ClickLoad()
    {

    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
