using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    UIManager UIManager;
    GameManager GameManager;

    float kills;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Player in scene");
            return;
        }

        instance = this;
        UIManager = GetComponent<UIManager>();
        GameManager = GetComponent<GameManager>();
    }

    public float health = 0;

    private void Start()
    {
        SetHealth(0);
        UIManager.SetKillsText(0.ToString());
    }

    public void SetHealth(float amount)
    {
        health += amount;
        UIManager.SetHealthText(health);
        if(health <= 0)
        {
            GameManager.EndGame();
        }
        else
        {
            GameManager.StartGame();
        }
    }

    public void AddKills()
    {
        kills += 1;
        UIManager.SetKillsText(kills.ToString());
    }
}
