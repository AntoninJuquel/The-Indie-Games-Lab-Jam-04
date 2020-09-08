using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TurretStatus))]
[RequireComponent(typeof(TurretFindingTarget))]
public class TurretAttacking : MonoBehaviour
{
    [Header("General")]
    public string soundFx;
    public int soundNb;
    public Transform partToRotate;
    public Transform firePoint;
    public float damage;
    public float rotationSpeed;

    [Header("Use projectile")]
    public GameObject projectile;
    public float fireRate;
    float fireRateTimer;
    public float projectileSpeed = 70f;

    [Header("Use Laser")]
    public bool useLaser;
    public GameObject laserPrefab;
    GameObject spawnedLaser;

    [Header("Mortar")]
    public bool launchBullet;


    [Header("Explosion")]
    public float explosionRadius;
    public float explosionDamage;


    TurretStatus myStatus;
    TurretFindingTarget turretFindingTarget;
    IDamageable damageable;
    // Start is called before the first frame update
    void Awake()
    {
        myStatus = GetComponent<TurretStatus>();
        turretFindingTarget = GetComponent<TurretFindingTarget>();
    }

    private void Start()
    {
        if (useLaser)
        {
            spawnedLaser = Instantiate(laserPrefab, firePoint.transform) as GameObject;
            DisableLaser();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (myStatus.status == TurretStatus.state.pending)
            if (useLaser)
            {

                DisableLaser();
                
            }
        AttackTarget();
    }

    void AttackTarget()
    {
        if (myStatus.status == TurretStatus.state.attacking)
        {
            WatchTarget();
            fireRateTimer += Time.deltaTime;
            if (fireRateTimer >= 1f / fireRate)
            {
                if (turretFindingTarget.target)
                {
                    damageable = turretFindingTarget.target.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        AudioManager.instance.Play(soundFx + Random.Range(0, soundNb));
                        if (useLaser)
                            Laser();
                        else if (launchBullet)
                            LaunchBullet();
                        else
                            Shoot();
                    }
                }

            }
        }
    }

    void WatchTarget()
    {
        if (turretFindingTarget.target != null)
        {
            Vector3 dir = turretFindingTarget.target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    void Shoot()
    {
        GameObject _projectile = (GameObject)Instantiate(projectile, firePoint.position, firePoint.rotation);
        Projectile p = _projectile.GetComponent<Projectile>();

        if (p != null)
            p.Seek(turretFindingTarget.target, projectileSpeed, damage, explosionRadius, explosionDamage);
        fireRateTimer = 0;
    }

    void Laser()
    {
        if (damageable != null)
        {
            damageable.Hit(damage * Time.deltaTime);
        }

        EnableLaser();
    }

    void EnableLaser()
    {
        spawnedLaser.SetActive(true);
        spawnedLaser.transform.position = firePoint.position;
        spawnedLaser.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(0, spawnedLaser.transform.position);
        spawnedLaser.transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(1, turretFindingTarget.target.position);
    }
    void DisableLaser()
    {
        spawnedLaser.SetActive(false);
    }

    void LaunchBullet()
    {
        Rigidbody _projectile = (Rigidbody)Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        _projectile.GetComponent<Projectile>().SetLaunchBullet(damage, explosionRadius);
        GetComponent<LaunchBall>().SetUpLaunch(_projectile, turretFindingTarget.target);
        GetComponent<LaunchBall>().Launch();
        fireRateTimer = 0;
    }
}
