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
                }
                navMesh.SetDestination(Player.transform.position);
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
    }
}
