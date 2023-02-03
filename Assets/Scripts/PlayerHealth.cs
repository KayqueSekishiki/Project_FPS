using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public Image bloodImage;
    public Image redImage;

    private Color alphaAmount;

    public int recoveryFactor;
    public float recoveryRate;
    private float recoveryTimer;

    public bool isDead = false;


    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        recoveryTimer += Time.deltaTime;
        if (recoveryTimer > recoveryRate)
        {
            StartCoroutine(RecoveryHealth());
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        alphaAmount = bloodImage.color;
        alphaAmount.a += ((float)damage / 100);

        bloodImage.color = alphaAmount;

        if (redImage.color.a < 0.1f)
        {
            redImage.color = new Color(255f, 0, 0, alphaAmount.a);
        }

        if (health <= 0)
        {
            GameController.GC.ShowGameOver();
            isDead = true;
        }

        recoveryTimer = 0f;
    }

    IEnumerator RecoveryHealth()
    {

        while (health < maxHealth)
        {
            health += recoveryFactor;

            alphaAmount.a -= ((float)recoveryFactor / 100);

            bloodImage.color = alphaAmount;
            redImage.color = new Color(255f, 0, 0, alphaAmount.a);
            yield return new WaitForSeconds(2f);
        }
    }
}
