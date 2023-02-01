using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100f;
    public int totalBullets = 30;
    public int bulletsLeft = 100;
    public int currentBullets;

    public float fireRate = 1f;
    private float firetimer;

    public Transform shootPoint;
    public ParticleSystem fireEffect;

    private Animator anim;

    private bool isReloading;


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
            else if (bulletsLeft > 0)
            {
                DoReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentBullets < totalBullets && bulletsLeft > 0)
            {
                DoReload();
            }
        }

        if (firetimer < fireRate || isReloading || currentBullets <= 0)
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

    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
    }

    private void DoReload()
    {
        if (isReloading)
        {
            return;
        }

        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    public void Reload()
    {
        if (bulletsLeft <= 0)
        {
            return;
        }

        int bulletsToLoad = totalBullets - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }
}
