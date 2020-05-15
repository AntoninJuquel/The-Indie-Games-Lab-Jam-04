using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float damage;
    private float explosionRadius;
    private float explosionDamage;
    private float speed = 70f;
    public GameObject impactEffect;

    public LayerMask whatIsEnemies;

    public bool launchBullet;

    public void SetLaunchBullet(float _damage, float _explosionRadius)
    {
        launchBullet = true;
        damage = _damage;
        explosionRadius = _explosionRadius;
    }
    public void Seek(Transform _target, float _speed, float _damage, float _explosionRadius, float _explosionDamage)
    {
        launchBullet = false;
        target = _target;
        speed = _speed;
        damage = _damage;
        explosionRadius = _explosionRadius;
        explosionDamage = _explosionDamage;
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null && !launchBullet)
        {
            Destroy(gameObject);
            return;
        }

        if (target)
        {
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (launchBullet)
        {
            GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(collision.contacts[0].point, explosionRadius, whatIsEnemies);
            foreach(Collider col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                if(damageable!=null)
                {
                    damageable.Hit(damage);
                }
            }
            AudioManager.instance.Play("Explosion" + Random.Range(0, 3));
            Destroy(effect, 5f);
            Destroy(gameObject);
        }
    }

    void HitTarget()
    {
        GameObject effect = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);

        Damage(damage, target);

        if (explosionRadius > 0)
            Explode();

        Destroy(effect, 5f);

        Destroy(gameObject);
    }

    void Damage(float _damage, Transform _target)
    {
        IDamageable damageable = _target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Hit(_damage);
        }
    }

    void Explode()
    {
        AudioManager.instance.Play("Explosion" + Random.Range(0, 3));
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRadius, whatIsEnemies);
        foreach (Collider enemy in enemies)
        {
            Damage(explosionDamage, enemy.transform);
        }
    }

}
