using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameOver;

    public static GameController GC;

    private void Start()
    {
        GC = this;
    }

    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
}
