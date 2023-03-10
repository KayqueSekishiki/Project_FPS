using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent navMesh;

    public ParticleSystem fireEffect;

    private GameObject Player;
    private PlayerHealth playerHealth;

    public float atkDistance = 10f;
    public float followDistance = 20f;
    public float atkProbality;

    public int damage;
    public int health;

    public Transform shootPoint;
    public float range = 120f;

    public float fireRate = 0.5f;
    private float fireTimer;

    public AudioClip shootAudio;
    private AudioSource audioSource;

    private bool isDead;


    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navMesh.enabled && !playerHealth.isDead)
        {
            float dist = Vector3.Distance(Player.transform.position, transform.position);
            bool shoot = false;
            bool follow = (dist < followDistance);

            if (follow)
            {
                if (dist < atkDistance)
                {
                    shoot = true;
                    Fire();
                }
                //  navMesh.isStopped = false;
                navMesh.SetDestination(Player.transform.position);
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
                shootPoint.LookAt(Player.transform);
            }

            if (!follow || shoot)
            {
                //  navMesh.isStopped = true;
                navMesh.SetDestination(transform.position);
            }
            anim.SetBool("shoot", shoot);
            anim.SetBool("run", follow);
        }

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void Fire()
    {

        if (fireTimer < fireRate)
        {
            return;
        }


        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, range))
        {
            //  Debug.Log(hit.transform.name);

            if (hit.transform.GetComponent<PlayerHealth>())
            {
                hit.transform.GetComponent<PlayerHealth>().ApplyDamage(damage);
            }
        }

        fireEffect.Play();
        PlayShootAudio();

        fireTimer = 0;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            navMesh.enabled = false;
            anim.SetBool("shoot", false);
            anim.SetBool("run", false);
            anim.SetTrigger("die");
            isDead = true;
        }
    }

    public void PlayShootAudio()
    {
        audioSource.PlayOneShot(shootAudio);
    }
}
