using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    public int value;
    CurrencyManager CurrencyManager;
    public CurrencyManager.Currency currency;

    public float timer;
    private void Start()
    {
        CurrencyManager = FindObjectOfType<CurrencyManager>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            GetMoney();
        }
    }

    private void OnMouseDown()
    {
        GetMoney();
    }

    void GetMoney()
    {
        string soundToPlay;
        if (gameObject.name.Contains("Coconut"))
            soundToPlay = "Coconut";
        else
            soundToPlay = "Seed";

        AudioManager.instance.Play(soundToPlay);
        
        CurrencyManager.SetMoney(value, currency);
        Destroy(gameObject);
    }
}
