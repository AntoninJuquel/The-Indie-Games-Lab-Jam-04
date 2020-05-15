using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palm : MonoBehaviour, IDamageable
{
    public GameObject coconut;
    public float height;
    public float matureHeight;
    public float coconutDropRate;

    public GameObject seed;
    public float chanceToDropSeed;

    public float growTime;
    float growTimer = 0;

    public GameObject[] palmTrees;
    public GameObject rend;

    Vector3 adultScale;
    private void Start()
    {
        rend = palmTrees[Random.Range(0, palmTrees.Length)];
        Instantiate(rend, transform.position, transform.rotation).transform.parent = this.transform;

        adultScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = adultScale * (height / matureHeight);

        Player.instance.SetHealth(GetComponent<Health>().health);

        InvokeRepeating("DropCoconut", growTime*(matureHeight-1) + .5f, coconutDropRate);
    }

    private void Update()
    {
        if (height < matureHeight)
        {
            growTimer += Time.deltaTime;
            if (growTimer >= growTime)
            {
                Grow();
                growTimer = 0;
            }
        }
    }

    public void Hit(float amount)
    {

        GetComponent<Health>().health -= amount;
        
        if (GetComponent<Health>().health <= 0)
        {
            Player.instance.SetHealth(-100);
            Die();
        }
    }

    public void Die()
    {
        BuildManager.instance.RemoveFromLists(transform);
        Destroy(gameObject);
    }

    public void Grow()
    {
        height++;
        transform.GetChild(0).localScale = adultScale * (height / matureHeight);
    }

    void DropCoconut()
    {
        if (height < matureHeight)
            return;

        Vector3 _whereToSpawn = new Vector3(Random.Range(-1f, 1f), height, Random.Range(-1f, 1f));

        Instantiate(coconut, transform.position + _whereToSpawn, Quaternion.identity);

        if(Random.value < chanceToDropSeed)
            Instantiate(seed,transform.position + _whereToSpawn, Quaternion.identity);

    }
}
