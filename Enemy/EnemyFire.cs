using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{

    private AudioSource audio;
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;
    private BulletTraile bulletTraile;
    [SerializeField]
    private Transform muzzlePoint;

    [SerializeField]
    private bool isBoss = false;

    [SerializeField] private int enemyDamage;
    [SerializeField] private int enemyAccuracy;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 0.0f;
    [SerializeField]
    private float fireRate = 0.7f;
    private readonly float damping = 10.0f;

    private readonly float reloadTime = 2.0f;
    private readonly int maxBullet = 10;
    private int currBullet = 10;
    private bool isReload = false;
    private WaitForSeconds wsReload;

    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip reloadSfx;
    
    public Transform firePos;
    public ParticleSystem muzzleFlash;

    private StatusController statusController;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        statusController = GameObject.FindObjectOfType<StatusController>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
        bulletTraile = GetComponent<BulletTraile>();

        wsReload = new WaitForSeconds(reloadTime);
    }


    void Update()
    {
        if (!isReload && isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        muzzleFlash.Play();
        if(isBoss)
            bulletTraile.BulletTrail(muzzlePoint.position, playerTr.position);

        if (enemyAccuracy> Random.Range(0,100))
            statusController.DecreaseHP(enemyDamage);

        isReload = ((--currBullet % maxBullet).Equals(0));
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        audio.PlayOneShot(reloadSfx, 1.0f);

        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }
}
