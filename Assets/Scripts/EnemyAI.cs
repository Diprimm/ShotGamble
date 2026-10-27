using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum AttackType {Melee, Ranged}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private AttackType attackType; 
    private NavMeshAgent agent;
    private Transform player;
    private Transform lastPlayerPosition;
    [SerializeField] private float attackRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private byte enemyDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool isHitscan;

    public GameObject projectilePrefab;
    public Transform firepointPosition;

    private float lastAttackTime;
    [SerializeField] private bool canSeeplayer;
    public LayerMask layerMask;

    [Header("Animations")]
    private Animator enemyAnimator;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(EnemyMovement());
    }

    private void Update()
    {       
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (Time.time >= lastAttackTime + attackCooldown && canSeeplayer)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }

        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        //transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));



        switch (attackType)
        {
            case AttackType.Melee:
                MeleeAttack();
                break;         
            case AttackType.Ranged:
                RangedAttack();
                break;
        }
    }

    private IEnumerator EnemyMovement()
    {
        while (player != null)
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetFloat("Velocity", agent.velocity.magnitude);
            }

            

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
                    if ( lastPlayerPosition != null)
                    {
                        agent.SetDestination(lastPlayerPosition.position);
                        canSeeplayer = false;
                    }
                }

            }

            else if (distanceToPlayer <= attackRange)
            {
                yield return new WaitForSeconds(0.8f);

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

            yield return new WaitForSeconds(0.1f);
        }
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


    private void MeleeAttack()
    {
        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(enemyDamage);
        }

        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("isShooting");
        }
    }

    private void RangedAttack()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("isShooting");
        }

        if (projectilePrefab != null && !isHitscan)
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

        else
        {
            player.gameObject.TryGetComponent(out PlayerHealth playerHealth);
            playerHealth.TakeDamage(enemyDamage);
        }
    }


}
