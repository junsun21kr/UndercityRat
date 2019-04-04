using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Type : MonoBehaviour
{
    public enum ObstacleType { BOX,WALL,OVERBOX}

    public ObstacleType obstacleType;

    public float offset;
}
