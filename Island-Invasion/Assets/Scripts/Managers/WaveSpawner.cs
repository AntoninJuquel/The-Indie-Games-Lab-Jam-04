using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public List<Transform[]> enemies;
    [Header("Enemies prefabs")]
    public Transform[] trashPrefab;
    public Transform[] rangePrefab;
    public Transform[] tankPrefab;
    public Transform[] splashRangePrefab;
    public Transform[] treeHunterPrefab;
    public Transform[] kamikazePrefab;
    public Transform[] summonerPrefab;

    [Header("Waves")]
    public Wave[] wave;

    [Header("Generals")]
    public float timeBtwWaves = 5.5f;
    public float countdown = 2f;
    public List<Transform> spawnPoints;

    [Header("Progress")]
    public int waveIndex = 0;


    [Header("Unity stuff")]
    public Transform _Dynamic;

    UIManager UIManager;
    CurrencyManager currencyManager;
    private void Awake()
    {
        UIManager = GetComponent<UIManager>();
        currencyManager = GetComponent<CurrencyManager>();
    }
    private void Start()
    {
        UIManager.SetWaveNumber(0.ToString());
        enemies = new List<Transform[]>
        {
            trashPrefab,
            rangePrefab,
            tankPrefab,
            splashRangePrefab,
            treeHunterPrefab,
            kamikazePrefab,
            summonerPrefab,
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameStarted)
        {
            if (EnemiesAlive > 0 || GameManager.gameEnded)
                return;
            else if (waveIndex == wave.Length)
                GameManager.instance.WinLevel();



            countdown -= Time.deltaTime;
            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
            UIManager.SetCountDownText(countdown);

            if (countdown <= 0f)
            {

                StartCoroutine(SpawnWave());
                countdown = timeBtwWaves;
            }
        }
    }
    IEnumerator SpawnWave()
    {
        NewWave();
        if (!GameManager.gameEnded)
        {
            foreach (Wave.Enemies enemie in wave[waveIndex - 1].enemiesToSpawn)
            {
                StartCoroutine(SpawnEnemy(enemie.index, enemie.number, enemie.level, enemie.spawnRate));
                yield return new WaitForSeconds(wave[waveIndex - 1].timeBtwGroups);
            }
        }

    }
    void NewWave()
    {
        if (GetComponent<EndLess>())
        {
            GetComponent<EndLess>().NewRandomWave();
            UIManager.SetWaveNumber(waveIndex.ToString());
            if (!GameManager.gameEnded)
                foreach (Wave.Enemies enemie in wave[0].enemiesToSpawn)
                {
                    EnemiesAlive += enemie.number;
                }
            return;
        }

        waveIndex++;
        UIManager.SetWaveNumber(waveIndex.ToString());
        if (!GameManager.gameEnded)
            foreach (Wave.Enemies enemie in wave[waveIndex - 1].enemiesToSpawn)
            {
                EnemiesAlive += enemie.number;
            }
        print(EnemiesAlive);

    }

    IEnumerator SpawnEnemy(int enemyIndex, int nb, int level, float spawnRate)
    {
        for (int i = 0; i < nb; i++)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Count);
            Transform newEnemy = Instantiate(enemies[enemyIndex - 1][level - 1], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            newEnemy.parent = _Dynamic;
            yield return new WaitForSeconds(1 / spawnRate);
        }
    }
}
