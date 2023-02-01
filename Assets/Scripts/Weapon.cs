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

    public GameObject hitEffect;
    public GameObject bulletImpact;

    private Animator anim;

    private bool isReloading;

    public AudioClip shootSound;
    private AudioSource audioSource;

    public enum ShootMode
    {
        Auto,
        Semi
    }

    public ShootMode shootMode;
    private bool shootInput;


    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentBullets = totalBullets;
    }


    void Update()
    {
        //if (Input.GetButton("Fire1"))
        //{
        //    if (currentBullets > 0)
        //    {
        //        Fire();
        //    }
        //    else if (bulletsLeft > 0)
        //    {
        //        DoReload();
        //    }
        //}

        switch (shootMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }

        if (shootInput)
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
            GameObject hitParticle = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bullet = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            Destroy(hitParticle, 1f);
            Destroy(bullet, 10f);
        }

        anim.CrossFadeInFixedTime("Fire", 0.01f);
        fireEffect.Play();
        PlayShootSound();
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

    void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
