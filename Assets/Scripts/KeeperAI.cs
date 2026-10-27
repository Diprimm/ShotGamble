using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Burst.CompilerServices;

public class KeeperAI : MonoBehaviour
{
    [Header("Enemy Config")]
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private byte enemyDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool catGrabbed;
    public float fearDistance = 5f; 
    public float fleeMultipler = 2f; 
    private float lastAttackTime;

    [Header("References")]
    [SerializeField] private Entity entityScript;
    private Transform player;
    private Transform lastPlayerPosition;
    public GameObject projectilePrefab;
    public GameObject catPickUpPrefab;
    public GameObject catPrefab;
    public Transform returnCatPosition;
    public Transform firepointPosition;
    private Transform cat;

    [Header("Enemy AI Config")]
    private NavMeshAgent agent;
    [SerializeField] private bool canSeeplayer;
    public LayerMask layerMask;
    public LayerMask catLayer;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        returnCatPosition = GameObject.Find("ReturnCatPosition").transform;
    }
   

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (cat == null)
        {
            ScanForCat();
        }

        if (cat != null && !catGrabbed)
        {
            GoToCat(cat);
        }

        if (catGrabbed)
        {
            agent.SetDestination(returnCatPosition.position);
                    
            entityScript.dieLoot = catPickUpPrefab;
        }

        else if (!catGrabbed && cat==null)
        {
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown && canSeeplayer && !catGrabbed)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }


            EnemyMovement();
                        
        }
        
    }

    private void FleeFromPlayer()
    {      
        Vector3 directionToPlayer = transform.position - player.position;
       
        Vector3 newDestination = transform.position + directionToPlayer.normalized * fleeMultipler;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(newDestination, out hit, fleeMultipler, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        else
        {
            Vector3 randomFlee = transform.position + (Random.insideUnitSphere * fleeMultipler);
            if (NavMesh.SamplePosition(randomFlee, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private void ScanForCat()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10, catLayer);

        if (hitColliders.Length > 0 && !catGrabbed)
        {
            cat = hitColliders[0].transform;
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        RangedAttack();
    }

    private void EnemyMovement()
    {
        if (!catGrabbed)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
            {
                if (InSight())
                {
                    if (player.position != null)
                    {
                        lastPlayerPosition = player;
                        agent.SetDestination(player.position);
                        canSeeplayer = true;
                    }
                }

                else
                {
                    if (lastPlayerPosition != null)
                    {
                        agent.SetDestination(lastPlayerPosition.position);
                        canSeeplayer = false;
                    }
                }

            }

            else if (distanceToPlayer <= attackRange)
            {
                if (InSight())
                {
                    lastPlayerPosition = player;
                    agent.SetDestination(transform.position);
                    canSeeplayer = true;
                }

                else
                {
                    if (lastPlayerPosition != null)
                    {
                        agent.SetDestination(lastPlayerPosition.position);
                        canSeeplayer = false;
                    }
                }
            }                
        }           
        
    }

    private void GoToCat(Transform cat)
    {
        agent.SetDestination(cat.transform.position);
    }

    private void GrabCat(Collider cat)
    {
        UIManager.instance.RefreshObjectiveUI("Don't let them scape with the cat!", Color.red);
        catGrabbed = true;
        catPrefab.SetActive(true);
        Destroy(cat.gameObject);
    }

    private bool InSight()
    {
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, chaseRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void RangedAttack()
    {
        if (projectilePrefab != null)
        {
            Vector3 fireDirection = (player.transform.position - firepointPosition.position).normalized;

            firepointPosition.rotation = Quaternion.LookRotation(fireDirection);

            GameObject newProjectile = Instantiate(projectilePrefab, firepointPosition.position, firepointPosition.rotation);
            if (newProjectile.TryGetComponent(out Projectile projScript))
            {
                projScript.projectileDamage = enemyDamage;
                projScript.speed = projectileSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && !catGrabbed)
        {
            GrabCat(other);
        }

        if (other.CompareTag("Return") && catGrabbed)
        {
            LevelManager.instance.LevelFailed();
            Debug.Log("YOU LOST, GAME OVER");
        }
    }
}
