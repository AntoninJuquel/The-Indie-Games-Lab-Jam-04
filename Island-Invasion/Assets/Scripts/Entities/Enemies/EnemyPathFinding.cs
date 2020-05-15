using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAttacking))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyPathFinding : MonoBehaviour
{

    NavMeshAgent agent;
    BuildManager buildManager;

    public enum Priority { any, tree, sniper, mortar, rocket, laser }
    public Priority priority;
    public Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        buildManager = BuildManager.instance;
    }
    private void Start()
    {
        agent.stoppingDistance = GetComponent<EnemyAttacking>().range;
        agent.speed = GetComponent<EnemyStatus>().moveSpeed;

        target = FindNearestTurret();
        if (target)
        {
            SetNavigation();
        }
    }

    private void Update()
    {
        if (GameManager.gameEnded || !GameManager.gameStarted)
            return;

        if (target == null)
        {
            target = FindNearestTurret();
            SetNavigation();
        }
    }

    void SetNavigation()
    {
        agent.SetDestination(target.position);
    }

    Transform FindNearestTurret()
    {
        float dist;
        Transform turret = null;
        switch (priority)
        {
            case Priority.any:
                dist = Mathf.Infinity;
                for (int i = 0; i < buildManager.turretsOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.turretsOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.turretsOnGround[i];
                    }
                }
                break;
            case Priority.laser:
                dist = Mathf.Infinity;
                if (buildManager.lasersOnGround.Count == 0)
                    break;
                for (int i = 0; i < buildManager.lasersOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.lasersOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.lasersOnGround[i];
                    }
                }
                break;
            case Priority.mortar:
                dist = Mathf.Infinity;
                if (buildManager.mortarsOnGround.Count == 0)
                    break;
                for (int i = 0; i < buildManager.mortarsOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.mortarsOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.mortarsOnGround[i];
                    }
                }
                break;
            case Priority.rocket:
                dist = Mathf.Infinity;
                if (buildManager.rocketsOnGround.Count == 0)
                    break;
                for (int i = 0; i < buildManager.rocketsOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.rocketsOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.rocketsOnGround[i];
                    }
                }
                break;
            case Priority.tree:
                dist = Mathf.Infinity;
                if (buildManager.treesOnGround.Count == 0)
                    break;
                for (int i = 0; i < buildManager.treesOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.treesOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.treesOnGround[i];
                    }
                }
                break;
            case Priority.sniper:
                dist = Mathf.Infinity;
                if (buildManager.snipersOnGround.Count == 0)
                    break;
                for (int i = 0; i < buildManager.snipersOnGround.Count; i++)
                {
                    float _newDist = Vector3.Distance(transform.position, buildManager.snipersOnGround[i].position);
                    if (_newDist < dist)
                    {
                        dist = _newDist;
                        turret = buildManager.snipersOnGround[i];
                    }
                }
                break;
            default:
                if (turret == null)
                    priority = Priority.any;
                break;
        }
        

        return turret;
    }

}
