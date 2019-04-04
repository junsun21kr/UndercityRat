using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    //순찰지점 저장 List타입 변수
    //public List<Transform> wayPoints;
    //다음 순찰 지점의 배열 인덱스
    //public int nextIdx;
    private readonly float patrolSpeed = 3.0f;
    private readonly float traceSpeed = 4.5f;
    private float damping = 1.0f;

    //NavMeshAgent 컴포넌트를 저장할 변수
    [SerializeField]
    private NavMeshAgent agent;
    private Transform enemyTr;
    private EnemyAI enemyAI;

    public void SetNavMeshOff()
    {
        agent.enabled = false;
        print("NavMeshOFF");
    }

    private bool _patrolling;
    public bool patrolling
    {
        get
        {
            return _patrolling;
        }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                TraceTarget(_traceTarget);
                //MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get
        {
            return _traceTarget;
        }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get
        {
            return agent.velocity.magnitude;
        }
    }

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        enemyAI = GetComponent<EnemyAI>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        TraceTarget(_traceTarget);


        //var group = GameObject.Find("WayPointGroup");

        /*if(group != null)
        {
            //WayPointGroup 하위에 모든 Transform 컴포넌트를 추출 후 List타입에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
            //배열 첫목록 삭제
            wayPoints.RemoveAt(0);

            nextIdx = Random.Range(0, wayPoints.Count);

        }
        MoveWayPoint();*/
    }

    /*void MoveWayPoint()
    {
        if (agent.isPathStale)
        {
            return;
        }

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }*/

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
        {
            return;
        }

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        if (enemyAI.isDie)
            return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update ()
    {
        //적 캐릭터가 이동중일때만 회전
        if (!enemyAI.isDie)
        {
            if (agent.isStopped.Equals(false))
            {
                //NavMeshAgent가 가야할 방향 벡터를 쿼터니언타입의 각도로 변환
                Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
                enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
            }
        }
	}
}
