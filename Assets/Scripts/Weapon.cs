using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100f;
    public int totalBullets = 30;
    public int bulletsLeft;

    public float fireRate = 0.1f;
    private float firetimer;

    public Transform shootPoint;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }

        if(firetimer < fireRate)
        {
            firetimer = +Time.deltaTime;
        }
    }

    private void Fire()
    {
        if(firetimer < fireRate)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }

        firetimer = 0f;
    }
}
