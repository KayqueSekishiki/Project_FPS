using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Properties")]
    public int damage;
    public float range = 100f;
    public int totalBullets = 30;
    public int bulletsLeft = 100;
    public int currentBullets;
    public float fireRate = 1f;
    private float firetimer;

    [Header("Shoot Config")]
    public Transform shootPoint;
    public ParticleSystem fireEffect;
    public GameObject hitEffect;
    public GameObject bulletImpact;

    private Animator anim;
    private bool isReloading;

    public enum ShootMode
    {
        Auto,
        Semi
    }

    public ShootMode shootMode;
    private bool shootInput;

    [Header("Sounds")]
    public AudioClip shootSound;
    private AudioSource audioSource;

    [Header("Aim")]
    public Vector3 aimPos;
    public float aimSpeed;
    private Vector3 originalPos;




    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentBullets = totalBullets;
        originalPos = transform.localPosition;
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

        ToAim();
    }

    //
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
            bullet.transform.SetParent(hit.transform);

            Destroy(hitParticle, 1f);
            Destroy(bullet, 10f);

            if (hit.transform.GetComponent<ObjectHealth>())
            {
                hit.transform.GetComponent<ObjectHealth>().ApplyDamage(damage);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.01f);
        fireEffect.Play();
        PlayShootSound();
        currentBullets--;
        firetimer = 0f;
    }

    //
    public void ToAim()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * aimSpeed);
        }
    }

    //
    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
    }

    //
    private void DoReload()
    {
        if (isReloading)
        {
            return;
        }

        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    //
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
    //
    void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
