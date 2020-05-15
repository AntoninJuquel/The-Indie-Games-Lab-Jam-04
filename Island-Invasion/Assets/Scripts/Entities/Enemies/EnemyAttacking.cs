using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public float damage;
    public float attackRate;
    float attackTimer;
    public float range;
    EnemyPathFinding pathFinding;
    public bool isKamikaze;

    [Header("Only Ranged")]
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed;
    public float explosionRadius;
    public float explosionDamage;
    private void Awake()
    {
        pathFinding = GetComponent<EnemyPathFinding>();
    }

    private void Update()
    {
        if (GameManager.gameStarted)
        {
            if (GameManager.gameEnded)
                return;



            if (IsInRange())
            {
                if (range > 2)
                    RangeAttack();
                else
                    MeleeAttack();
            }
        }
    }

    bool IsInRange()
    {
        float dist = Mathf.Infinity;
        if (GetComponent<Transform>())
            dist = Vector3.Distance(transform.position, pathFinding.target.position);

        return dist < range;
    }

    void MeleeAttack()
    {
        IDamageable damageable = pathFinding.target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > 1f / attackRate)
            {
                StartCoroutine(Shader());
                AudioManager.instance.Play("EnemyAttack" + Random.Range(1, 9));
                damageable.Hit(damage);
                if (isKamikaze)
                    GetComponent<EnemyStatus>().Die();
                attackTimer = 0;
            }
        }
    }

    IEnumerator Shader()
    {
        meshRenderer.material.SetFloat("_noiseScale", 15);

        yield return new WaitForSeconds(0.5f);

        meshRenderer.material.SetFloat("_noiseScale", 5);
    }

    void RangeAttack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > 1f / attackRate)
        {
            StartCoroutine(Shader());
            AudioManager.instance.Play("EnemyAttack" + Random.Range(1, 9));
            GameObject _projectile = (GameObject)Instantiate(projectile, firePoint.position, firePoint.rotation);
            Projectile p = _projectile.GetComponent<Projectile>();

            if (p != null)
                p.Seek(pathFinding.target, projectileSpeed, damage, explosionRadius, explosionDamage);
            attackTimer = 0;
        }
    }
}
