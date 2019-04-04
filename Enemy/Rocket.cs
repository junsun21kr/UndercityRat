using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed;

    public GameObject ExplosionFx;

    private Transform target;

    private Rigidbody rigidbody;

    public int missileHp=4;

    private bool isBoom=false;

    private AudioSource audioSource;

    [SerializeField]
    private ParticleSystem particleSystem;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        speed += Random.Range(-5, 5);

        StartCoroutine(targetfind());
    }

    IEnumerator targetfind()
    {
        yield return new WaitForSeconds(3.0f);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void RocketFire()
    {
        audioSource.Play();
        rigidbody.isKinematic = false;
        var e = particleSystem.emission;
        e.enabled = true;
        StartCoroutine(LookOnTarget());
    }

    IEnumerator LookOnTarget()
    {
        while (!isBoom)
        {
            if (missileHp <= 0)
            {
                Boom();
            }
            var lookVector = target.position - transform.position;
            var lookRotation = Quaternion.LookRotation(lookVector.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 4f);
            rigidbody.velocity = transform.forward * speed;
            yield return null;
        }
        
    }

    public void HitRocket(int damage)
    {
        missileHp -= damage;
        print(missileHp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Boom();
    }

    private void Boom()
    {
        isBoom = true;
        if (Vector3.Distance(transform.position, target.position) < 6f)
        {
            int damage = 800 - (int)(Vector3.Distance(transform.position, target.position) * 100);
            GameObject.FindObjectOfType<StatusController>().DecreaseHP(damage);
            target.GetComponent<Rigidbody>().AddForce(-(transform.position - target.position).normalized * 150f);
        }
        Instantiate(ExplosionFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
