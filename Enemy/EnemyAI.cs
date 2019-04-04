using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    //적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL, TRACE, ATTACK,FLEE,HEAL, DIE
    }

    //상태를 저장할 변수
    public State state = State.PATROL;
    Transform playerTr;
    Transform enemyTr;
    private Animator animator;

    public float attackDist = 20.0f;
    public float traceDist = 35.0f;

    public bool isDie = false;
    private int isFlee = 1;
    private bool onHealing = false;
    WaitForSeconds ws;
    //이동을 제어하는 MoveAgent 클래스를 저장할 변수
    MoveAgent moveAgent;
    private EnemyFire enemyFire;
    private EnemyFOV enemyFOV;
    private EnemyController enemyController;

    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }

        enemyTr = GetComponent<Transform>();
        enemyController = GetComponent<EnemyController>();
        animator = GetComponentInChildren<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyFire = GetComponent<EnemyFire>();
        enemyFOV = GetComponent<EnemyFOV>();

        ws = new WaitForSeconds(0.3f);
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        StatusController.OnPlayerDie += this.OnPlayerDie;
    }
    void OnDisable()
    {
        StatusController.OnPlayerDie -= this.OnPlayerDie;
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(2.0f);

        while (!isDie)
        {
            if (state.Equals(State.DIE))
            {
                yield break;
            }

            float dist = Vector3.Distance(playerTr.position , enemyTr.position);
            if (!onHealing)
            {
                if (enemyController.currentHP < enemyController.maxHP * 0.4 && isFlee > 0)
                {
                    state = State.FLEE;
                    Invoke("Fleeminus", 5f);
                }
                else if (dist <= attackDist)
                {
                    if (enemyFOV.isViewPlayer())
                    {
                        state = State.ATTACK;
                    }
                    else
                    {
                        state = State.TRACE;
                    }
                }
                else if (enemyFOV.isTracePlayer())
                {
                    state = State.TRACE;
                }
                else
                {
                    state = State.PATROL;
                }
            }
            else
            {
                state = State.HEAL;
            }
            

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.FLEE:
                    print("도망중");
                    enemyFire.isFire = false;
                    Vector3 dirToPlayer = transform.position - playerTr.position;
                    moveAgent.traceTarget = transform.position + dirToPlayer;
                    animator.SetBool(hashMove, true);
                    break;
                case State.HEAL:
                    enemyFire.isFire = false;
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    if (enemyFire.isFire.Equals(false))
                    {
                        enemyFire.isFire = true;
                    }
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    moveAgent.SetNavMeshOff();
                    moveAgent.Stop();
                    GetComponent<CapsuleCollider>().enabled = false;
                    GetComponent<SphereCollider>().enabled = false;
                    GetComponent<Rigidbody>().useGravity = false;
                    break;
            }
        }
    }
    void Update()
    {
        //Speed 파라미터에 이동속도를 전달
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines();

        animator.SetTrigger(hashPlayerDie);
    }

    private void Fleeminus()
    {
        isFlee--;
        if (enemyController.healKit > 0)
        {
            enemyController.healKit--;
            StartCoroutine(HealingUpCoroutin());
        }
    }

    IEnumerator HealingUpCoroutin()
    {
        onHealing = true;
        animator.SetTrigger("Heal");
        yield return new WaitForSeconds(4.0f);
        enemyController.currentHP += (int)(enemyController.maxHP * 0.3f);
        onHealing = false;

        moveAgent.traceTarget = playerTr.position;
    }
}