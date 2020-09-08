using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFindingTarget : MonoBehaviour
{

    public Transform target;
    public float range = 15f;

    public LayerMask whatIsEnemy;


    bool targetFound;

    TurretStatus myStatus;
    // Start is called before the first frame update
    void Awake()
    {
        myStatus = GetComponent<TurretStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (range > 0)
        {
            if (targetFound == false)
            {
                UpdateTarget();
            }
            else if (myStatus.status == TurretStatus.state.pending)
            {
                targetFound = false;
            }
        }

        if(target)
            if(Vector3.Distance(transform.position,target.position)>range)
            {
                targetFound = false;
                target = null;
            }
    }

    void UpdateTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, whatIsEnemy);
        if (colliders.Length > 0)
        {
            float closestCol = Mathf.Infinity;
            target = null;

            foreach (Collider col in colliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestCol)
                {
                    closestCol = distance;
                    target = col.transform;
                }
            }
            if (target != null)
                targetFound = true;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
