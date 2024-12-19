using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonsScript : MonoBehaviour
{
    [Header("Settings")]
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float stoppingDistance = 1f;
    public float attackCooldown = 2f;
    public int damage = 10;
    public float spawnDelay = 1f;
    public float staggeredSpawnDelay = 2f;
    public int maxSkeletonCount = 10; // Maximum number of skeletons allowed globally

    [Header("References")]
    public Transform player;

    private List<GameObject> spawnedSkeletons = new List<GameObject>();
    private float attackTimer;
    private bool hasSpawned = false;
    private int totalSpawnedSkeletons = 0; // Tracks the total number of skeletons spawned

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
        }
    }

    void Update()
    {
        UpdateSkeletonsBehavior();
        attackTimer -= Time.deltaTime;
    }

    private void SpawnSkeletonsWithDelay()
    {
        if (hasSpawned || totalSpawnedSkeletons >= maxSkeletonCount) return;
        hasSpawned = true;
        StartCoroutine(SpawnSkeletonsCoroutine());
    }

    private IEnumerator SpawnSkeletonsCoroutine()
    {
        if (spawnPoints.Length == 0) yield break;

        int totalSkeletons = spawnPoints.Length;
        int batchSize = Mathf.CeilToInt(totalSkeletons / 2f);

        for (int i = 0; i < totalSkeletons; i++)
        {
            // Check if the global spawn limit is reached
            if (totalSpawnedSkeletons >= maxSkeletonCount)
            {
                Debug.Log("Global spawn limit reached. No more skeletons will be spawned.");
                yield break;
            }

            if (i < batchSize)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
            else
            {
                yield return new WaitForSeconds(staggeredSpawnDelay);
            }

            Transform spawnPoint = spawnPoints[i];
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPoint.position, Quaternion.identity, transform);

            NavMeshAgent agent = skeleton.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                agent = skeleton.AddComponent<NavMeshAgent>();
            }

            agent.stoppingDistance = stoppingDistance;
            agent.avoidancePriority = Random.Range(30, 60); // Randomize priority to reduce clustering

            Rigidbody rb = skeleton.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotations
            }

            SkeletonHealth skeletonHealth = skeleton.GetComponent<SkeletonHealth>();
            if (skeletonHealth != null)
            {
                skeletonHealth.manager = this;
            }

            spawnedSkeletons.Add(skeleton);
            totalSpawnedSkeletons++; // Increment the global spawn counter
        }
    }

    private void UpdateSkeletonsBehavior()
    {
        if (player == null) return;

        for (int i = spawnedSkeletons.Count - 1; i >= 0; i--)
        {
            GameObject skeleton = spawnedSkeletons[i];
            if (skeleton == null)
            {
                spawnedSkeletons.RemoveAt(i);
                continue;
            }

            NavMeshAgent agent = skeleton.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                float distanceToPlayer = Vector3.Distance(skeleton.transform.position, player.position);

                if (distanceToPlayer <= detectionRadius)
                {
                    if (distanceToPlayer > attackRange)
                    {
                        agent.isStopped = false;
                        agent.SetDestination(player.position);
                    }
                    else
                    {
                        agent.isStopped = true;
                        TryAttack(skeleton);
                    }
                }
                else
                {
                    agent.ResetPath();
                }
            }
        }
    }

    private void TryAttack(GameObject skeleton)
    {
        if (attackTimer <= 0f)
        {
            Debug.Log(skeleton.name + " attacks the player!");
            attackTimer = attackCooldown;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnSkeletonsWithDelay();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited detection area.");
            hasSpawned = false;
        }
    }

    public void RemoveSkeleton(GameObject skeleton)
    {
        if (spawnedSkeletons.Contains(skeleton))
        {
            spawnedSkeletons.Remove(skeleton);
        }
    }
}