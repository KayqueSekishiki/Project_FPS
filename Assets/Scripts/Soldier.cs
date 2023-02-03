using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent navMesh;
    private GameObject Player;

    public float atkDistance = 10f;
    public float followDistance = 20f;
    public float atkProbality;

    public int damage;
    public int health;

    public Transform shootPoint;
    public float range = 120f;

    public float fireRate = 0.5f;
    private float fireTimer;


    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (navMesh.enabled)
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
                navMesh.SetDestination(Player.transform.position);
                transform.LookAt(Player.transform);
                //  navMesh.isStopped = false;
            }

            if (!follow || shoot)
            {
                navMesh.SetDestination(transform.position);
                //  navMesh.isStopped = true;
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

        fireTimer = 0;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            navMesh.enabled = false;
            anim.SetBool("shoot", false);
            anim.SetBool("run", false);
            anim.SetTrigger("die");
        }
    }
}
