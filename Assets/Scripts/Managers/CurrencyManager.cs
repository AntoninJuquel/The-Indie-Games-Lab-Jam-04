using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static int Money;
    public static int Seed;
    public int startMoney = 400;
    public int startSeed = 1;
    public enum Currency
    {
        coconut,
        seed
    }
    UIManager UIManager;


    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
    }
    private void Start()
    {
        Money = startMoney;
        Seed = startSeed;
        UIManager.SetMoneyText(Money.ToString());
        UIManager.SetSeedText(Seed.ToString());
    }

    public void SetMoney(int _amount,Currency _currency)
    {
        switch (_currency)
        {
            case Currency.coconut:
                Money += _amount;
                UIManager.SetMoneyText(Money.ToString());
                break;
            case Currency.seed:
                Seed += _amount;
                UIManager.SetSeedText(Seed.ToString());
                break;
        }
    }

    public bool EnoughMoney(int cost, Currency _currency)
    {
        int checker = 0;

        switch (_currency)
        {
            case Currency.coconut:
                checker = Money - cost;
                break;
            case Currency.seed:
                checker = Seed - cost;
                break;
        }

        return checker >= 0;
    }
}
