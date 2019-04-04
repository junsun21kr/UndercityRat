using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyController;

    private Rocket[] rockets;

    private bool[] rocketFire;

    private bool allRocketDone;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rockets = GameObject.FindObjectsOfType<Rocket>();
    }

    private void Update()
    {
        if (!allRocketDone)
        {
            if (enemyController.currentHP < enemyController.maxHP * 0.7 && rocketFire[0] == false)
            {
                rocketFire[0] = true;
                rockets[0].RocketFire();
            }
            else if (enemyController.currentHP < enemyController.maxHP * 0.6 && rocketFire[1] == false)
            {
                rocketFire[1] = true;
                rockets[1].RocketFire();
            }
            else if (enemyController.currentHP < enemyController.maxHP * 0.5 && rocketFire[2] == false)
            {
                rocketFire[2] = true;
                rockets[2].RocketFire();
                rockets[3].RocketFire();
            }
            else if (enemyController.currentHP < enemyController.maxHP * 0.3)
            {
                rockets[4].RocketFire();
                rockets[5].RocketFire();
                rockets[6].RocketFire();
                rockets[7].RocketFire();
                allRocketDone = true;
            }
        }
        
    }
}
