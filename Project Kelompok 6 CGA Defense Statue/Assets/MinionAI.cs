using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player, statue;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint, distanceToWalkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float walkSpeed;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        // agent.baseOffset = 1f - 0.0833333f;
        agent.speed = walkSpeed;
    }

    private void Update(){
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        // Debug.Log(playerInSightRange+" "+playerInAttackRange+" "+walkPointSet);
        if(!playerInSightRange && !playerInAttackRange) ToStatue();
        else if(playerInSightRange && !playerInAttackRange) ToPlayer();
        else if(playerInAttackRange) AttackPlayer();
    }

    private void ToPlayer(){

    }

    private void AttackPlayer(){
        
    }

    private void ToStatue(){
        if(!walkPointSet) SetMoveToStatue();
        else if(distanceToWalkPoint.magnitude > attackRange) agent.SetDestination(walkPoint);
        else agent.isStopped = true;
        distanceToWalkPoint = transform.position - walkPoint;
    }

    private void SetMoveToStatue(){
        NavMeshHit hit;
        if(NavMesh.SamplePosition(statue.position, out hit, 1.0f, NavMesh.AllAreas)){
            walkPoint = hit.position;
            walkPointSet = true;
        }
        Debug.Log(NavMesh.SamplePosition(statue.position, out hit, 1.0f, NavMesh.AllAreas));
    }
}
