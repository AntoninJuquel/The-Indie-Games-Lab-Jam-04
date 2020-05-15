using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLess : MonoBehaviour
{

    /* for endless mode*/
    public int minGroup = 1;
    public int maxGroup = 1;
    public int minIndex = 1;
    public int maxIndex = 1;
    public int minNumber = 3;
    public int maxNumber = 5;
    public int minLevel = 1;
    public int maxLevel = 1;
    public int minSpawnRate = 1;
    public int maxSpawnRate = 1;


    public WaveSpawner waveSpawner;
    public UIManager UIManager;
    public CurrencyManager currencyManager;

    List<Wave.Enemies> enemies;
    private void Start()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        UIManager = GetComponent<UIManager>();
        currencyManager = GetComponent<CurrencyManager>();
    }
    /* To have endless mode*/
    public void NewRandomWave()
    {
        waveSpawner.waveIndex++;
        UIManager.SetWaveNumber(waveSpawner.waveIndex.ToString());

        if (waveSpawner.waveIndex % 3 == 0)
            AddSeed();

        SetDifficulty();
        List<Wave.Enemies> _enemies = new List<Wave.Enemies>();
        for (int i = 0; i < Random.Range(minGroup, maxGroup); i++)
        {
            _enemies.Add(new Wave.Enemies(Random.Range(minIndex, maxIndex), Random.Range(minNumber, maxNumber), Random.Range(minLevel, maxLevel), Random.Range(minSpawnRate, maxSpawnRate)));
        }
        enemies = _enemies;
        waveSpawner.wave.SetValue(new Wave(3f, enemies), 0);
    }

    public void AddSeed()
    {
        currencyManager.SetMoney(1, CurrencyManager.Currency.seed);
    }

    public void SetDifficulty()
    {
        if (waveSpawner.waveIndex % 2 == 0)
        {
            maxNumber++;
            if (maxIndex < enemies.Count)
                maxIndex++;
        }

        if (waveSpawner.waveIndex % 4 == 0)
        {
            maxGroup++;
        }

        if (waveSpawner.waveIndex > 14 && waveSpawner.waveIndex % 5 == 0)
            if (maxLevel < 4)
                maxLevel++;
        if (waveSpawner.waveIndex > 20 && waveSpawner.waveIndex % 5 == 0)
            if (minLevel < 4)
                minLevel++;

        if (waveSpawner.waveIndex % 10 == 0)
        {
            minGroup++;
            minNumber++;

        }
    }
}
