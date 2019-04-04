using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public enum BGM
{
    TitleBGM,MainBGM,ActionBGM,NonActionBGM
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
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

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    } 

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name.Equals(effectSounds[i].name))
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                print("모든 가용 오디오소스가 사용중입니다");
                return;
            }
        }
        print(_name + "사운드가 사운드매니저에 등록되지 않았습니다");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i].Equals(_name))
            {
                audioSourceEffects[i].Stop();
                return;
            }
            
        }
    }

    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void PlayTitleBGM(int bgmNum)
    {
        audioSourceBGM.clip = bgmSounds[bgmNum].clip;
        audioSourceBGM.Play();
    }

    public void BGMChangeTime(int bgmNum)
    {
        StartCoroutine(DecreaseBGM(bgmNum));
    }

    IEnumerator DecreaseBGM(int bgmNum)
    {
        for (int i = 0; i < 120; i++)
        {
            audioSourceBGM.volume -= 0.01f;
            yield return null;
        }
        StopBGM();
        PlayTitleBGM(bgmNum);
        StartCoroutine(IncreaseBGM());
    }

    IEnumerator IncreaseBGM()
    {
        for (int i = 0; i < 120; i++)
        {
            audioSourceBGM.volume += 0.01f;
            yield return null;
        }
    }
}
