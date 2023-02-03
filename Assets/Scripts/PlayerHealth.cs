using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public Image bloodImage;
    public Image redImage;

    private Color alphaAmount;

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
            Debug.Log("Game Over!");
        }
    }


}
