using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform currentTarget;

    public string enemyMinionTag = "EnemyMinion";
    public string turretTag = "Turret";
    public float stopDistance = 2.0f;
    public float aggroRange = 5.0f; // Adjust the range at which minions detect enemies.
    public float targetSwitchInterval = 2.0f; // Adjust the interval for checking and switching targets.

    private float timeSinceLastTargetSwitch = 0.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindAndSetTarget();
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);

            // Stop moving if within range of the target.
            if (Vector3.Distance(transform.position, currentTarget.position) <= stopDistance)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }

        // Check for a new target periodically.
        timeSinceLastTargetSwitch += Time.deltaTime;
        if (timeSinceLastTargetSwitch >= targetSwitchInterval)
        {
            timeSinceLastTargetSwitch = 0.0f;
            CheckAndSwitchTargets();
        }
    }

    private void CheckAndSwitchTargets()
    {
        // Search for the closest enemy minion or turret within aggroRange.
        GameObject[] enemyMinions = GameObject.FindGameObjectsWithTag(enemyMinionTag);
        Transform closestEnemy = GetClosestObjectInRadius(enemyMinions, aggroRange);

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
            return;
        }

        // If no enemy minions are in range, check for turrets.
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
        Transform closestTurret = GetClosestObjectInRadius(turrets, aggroRange);

        if (closestTurret != null)
        {
            currentTarget = closestTurret;
        }
    }

    private Transform GetClosestObject(GameObject[] objects)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj.transform;
            }
        }

        return closest;
    }

    private Transform GetClosestObjectInRadius(GameObject[] objects, float radius)
    {
        Transform closest = null;
        float closestDistance = radius;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= radius && distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj.transform;
            }
        }

        return closest;
    }

    private void FindAndSetTarget()
    {
        // Check for enemy minions first.
        GameObject[] enemyMinions = GameObject.FindGameObjectsWithTag(enemyMinionTag);
        Transform closestEnemy = GetClosestObjectInRadius(enemyMinions, aggroRange);

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
            return;
        }

        // If no enemy minions are found, check for turrets.
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
        Transform closestTurret = GetClosestObjectInRadius(turrets, aggroRange);

        if (closestTurret != null)
        {
            currentTarget = closestTurret;
        }
    }
}
