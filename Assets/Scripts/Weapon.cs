using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100f;
    public int totalBullets = 30;
    public int bulletsLeft;
    public int currentBullets;

    public float fireRate = 1f;
    private float firetimer;

    public Transform shootPoint;
    public ParticleSystem fireEffect;

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        currentBullets = totalBullets;
    }


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (currentBullets > 0)
            {
                Fire();
            }
        }

        if (firetimer < fireRate)
        {
            firetimer += Time.deltaTime;
        }
    }

    private void Fire()
    {
        if (firetimer < fireRate)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
           // Debug.Log(hit.transform.name);
        }

        anim.CrossFadeInFixedTime("Fire", 0.01f);
        fireEffect.Play();
        currentBullets--;
        firetimer = 0f;
    }
}
