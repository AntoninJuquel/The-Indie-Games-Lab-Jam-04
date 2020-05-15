using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyPathFinding))]
public class EnemyStatus : MonoBehaviour, IDamageable
{
    public string enemyName;
    public int level;

    public float moveSpeed;

    public GameObject deathEffect;

    EnemyHealthBar enemyHealthBar;

    public GameObject[] drops;
    public float seedDropChance;
    public float coconuDropChance;

    bool killAdded = false;

    [Header("Splitter")]
    public GameObject trashPrefab;
    public bool isSplitter;
    public int amountToSpawn;
    private void Start()
    {
        if (GetComponent<EnemyHealthBar>())
        {
            enemyHealthBar = GetComponent<EnemyHealthBar>();
            enemyHealthBar.SetUi(GetComponent<Health>().health, level, enemyName);
        }
    }

    public void Hit(float amount)
    {

        GetComponent<Health>().health -= amount;
        if (enemyHealthBar)
        {
            enemyHealthBar.UpdateHealthBar(GetComponent<Health>().health);
        }
        if (GetComponent<Health>().health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

        if (!killAdded)
        {
            float chance = Random.value;
            if (chance < seedDropChance)
                Instantiate(drops[1], transform.position, transform.rotation);
            else if (chance < coconuDropChance)
                Instantiate(drops[0], transform.position, transform.rotation);


            WaveSpawner.EnemiesAlive--;
            print(WaveSpawner.EnemiesAlive);
            Player.instance.AddKills();
            killAdded = true;

            if (isSplitter)
            {
                WaveSpawner.EnemiesAlive += amountToSpawn;
                for (int i = 0; i < amountToSpawn; i++)
                {
                    Instantiate(trashPrefab, transform.position, transform.rotation);
                }

            }
        }
        GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}
