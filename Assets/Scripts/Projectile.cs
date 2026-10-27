using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileDamage;
    public float projectileSize = 0.3f;
    public float explosionRadius = 0.3f;
    public float speed;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {      
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        if (Physics.SphereCast(transform.position, projectileSize, transform.forward, out RaycastHit hit, moveDistance))
        {
            OnHit();
        }

        else
        {
            //Si no detecta nada el proyectis se mueve hacia adelante;
            transform.Translate(Vector3.forward * moveDistance);
        }           
    }

    public void AimDirection(Vector3 direction)
    {
        transform.LookAt(direction);
    }
    private void OnHit()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, entityLayer);

        foreach (Collider collider in colliders)
        {
            if (colliders.Length > 0)
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, explosionRadius))
                {
                    if (hit.collider.TryGetComponent(out Entity entity))
                    {
                        Debug.Log("hit " + hit.collider.name);
                        entity.TakeDamage(projectileDamage);
                    }
                    
                    if (hit.collider.TryGetComponent(out PlayerHealth playerhealth))
                    {
                        Debug.Log("hit " + hit.collider.name);
                        playerhealth.TakeDamage(projectileDamage);
                    }
                }
            }
        }

        // Instantiate(explosionPrefab, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(gameObject);
    }
}
