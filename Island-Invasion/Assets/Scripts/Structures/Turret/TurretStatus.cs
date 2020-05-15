using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(TurretFindingTarget))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class TurretStatus : MonoBehaviour, IDamageable
{
    public enum state {pending,attacking}
    public state status;

    public enum Category { any,tree, sniper, mortar, rocket, laser }
    public Category category;

    public Ground sittingOn;



    [HideInInspector]
    public StructuresBlueprint myInfo;

    public int level = 1;

    TurretFindingTarget turretFindingTarget;

    // Start is called before the first frame update
    void Awake()
    {
        turretFindingTarget = GetComponent<TurretFindingTarget>();
        status = state.pending;
    }

    // Update is called once per frame
    void Update()
    {
        if (turretFindingTarget.target)
        {
            status = state.attacking;
        }
        else
        {
            status = state.pending;
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        SelectThisTurret();
    }

    public void SelectThisTurret()
    {
        BuildManager.instance.SelectTurret(gameObject,myInfo, level, GetComponent<Health>().health, GetComponent<TurretAttacking>().damage, GetComponent<TurretAttacking>().fireRate, GetComponent<TurretFindingTarget>().range);
    }

    public void Hit(float amount)
    {
        GetComponent<Health>().health -= amount;
        if(GetComponent<Health>().health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        BuildManager.instance.RemoveFromLists(transform);
        sittingOn.StructureDestroyed();
        Destroy(gameObject);
    }
}
