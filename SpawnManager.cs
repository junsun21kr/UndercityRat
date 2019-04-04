using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject[] enemyPrefabs;
    public float createTime;
    public int maxEnemy = 12;
    public int spawnDice=50;

    private int bossSpawn = 0;

    public bool OnAction = false;

    public bool FirstEntry = false;

    private void Start()
    {
        if (points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
        StartCoroutine(enemyCheck());
    }

    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(2f);
        while (!GameManager.instance.isGamaOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (spawnDice > Random.Range(1, 100))
            {
                if (enemyCount < maxEnemy)
                {                    
                    int idx = Random.Range(1, points.Length);
                    SpawnEnemy(idx);

                    yield return new WaitForSeconds(createTime);
                }
                else
                {
                    yield return new WaitForSeconds(createTime);
                }
            }
            else
            {
                spawnDice += 10;
                yield return new WaitForSeconds(createTime);
            }

        }
    }

    private void SpawnEnemy(int idx)
    {
        Transform[] spawnPoints = points[idx].GetComponentsInChildren<Transform>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Instantiate(enemyPrefabs[0], spawnPoints[i].position, points[idx].rotation);
        }
        Instantiate(enemyPrefabs[1], spawnPoints[0].position, points[idx].rotation);
        Instantiate(enemyPrefabs[1], spawnPoints[1].position, points[idx].rotation);

        bossSpawn += 6;
        if(bossSpawn > 10)
        {
            Instantiate(enemyPrefabs[2], spawnPoints[0].position, points[idx].rotation);
            bossSpawn = 0;
        }

        spawnDice = 50;
    }

    IEnumerator enemyCheck()
    {
        while (!GameManager.instance.isGamaOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;

            if(enemyCount > 0)
            {
                OnAction = true;
                if(SoundManager.instance.audioSourceBGM.clip != SoundManager.instance.bgmSounds[2].clip)
                {
                    SoundManager.instance.BGMChangeTime(2);
                }
            }
            else
            {
                OnAction = false;
                if (SoundManager.instance.audioSourceBGM.clip != SoundManager.instance.bgmSounds[3].clip)
                {
                    SoundManager.instance.BGMChangeTime(3);
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
