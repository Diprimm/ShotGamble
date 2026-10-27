using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public UnityEvent OnGunShot;

    [Header("Gun Settings")]
    [SerializeField] private bool isAutomatic;
    [SerializeField] private int gunDamage;
    [SerializeField] private float gunCooldown;

    [Header("Shotgun Settings")]
    [SerializeField] private bool isShotgun;
    [SerializeField] private int pelletsCount = 5;
    [SerializeField] private float spreadAngle = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileSize;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float hitscanRange = 40;

    [Header("Ammo Settings")]
    public AmmoType ammoType;
    [SerializeField] private int ammoPerShot = 1;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilAmount = 0.1f;
    [SerializeField] private float recoverSpeed = 5f;
    [SerializeField] private float rotationRecoilAmount = 2f;
    //Desplazamiento acumulado en este momento.
    private Vector3 recoilOffset;

    [Header("Particles")]
    public ParticleSystem bulletHitWallParticles;
    public ParticleSystem muzzleFlashParticles;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float rotationShakeMagnitude;
    

    [Header("References")]
    public Transform playerCamera;
    public Transform projectileSpawnPoint;
    public Transform muzzleFlashPosition;
    [SerializeField] private AmmoManager ammoManager;
    public PlayerController playerController;
    private CameraShake camShake;
    private float currentCooldown;
    private Vector3 startPos;
    public LayerMask layerMask;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        ammoManager = GetComponentInParent<AmmoManager>();

        if (ammoManager == null)
        {
            Debug.LogError($"¡Error! No se encontró un AmmoManager en los padres de {gameObject.name}. " +
                           "Asegúrate de que el objeto Jugador tenga el script AmmoManager.");
        }
    }
    private void Start()
    {
        camShake = Camera.main.GetComponent<CameraShake>();
        startPos = transform.localPosition;
        currentCooldown = 0.2f;
        playerCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        projectileSpawnPoint = GameObject.Find("ProjectileSpawnPoint").GetComponent<Transform>();
    }

    private void OnEnable()
    {
        Quaternion rotationX = Quaternion.AngleAxis(35, Vector3.right);
        transform.localRotation = rotationX;
    }


    private void Update()
    {
        HandleInput();

        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;

        //Recuperación de la posición con LERP.
        //El vector recoilOffset vuelve lentamente a Vector3.zero (sin offset), a velocidad de recoverSpeed.
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, recoverSpeed *  Time.deltaTime);

        //Se aplica el desplazamiento del retroceso.
        transform.localPosition = startPos + recoilOffset;


        //Constantemente el arma vuelve a su rotacion inicial
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 15 * Time.deltaTime);
    }

    public void ApplyRecoil()
    {
        //Se añade cambio al offset simulando el retroceso.
        recoilOffset += new Vector3(0, recoilAmount * 0.5f, -recoilAmount);

        Quaternion rotationX = Quaternion.AngleAxis(-rotationRecoilAmount, Vector3.right);

        transform.localRotation = rotationX;
    }

    private void HandleInput()
    {
        bool inputDetected = isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);

        if (inputDetected && currentCooldown <= 0)
        {
            if (ammoManager.HasAmmo(ammoType, ammoPerShot))
            {
                Shoot();
                ApplyRecoil();
            }

            else
            {
                WeaponManager.instance.ChangeWeapon(1);
            }
        }
    }

    private void Shoot()
    {
        if (muzzleFlashParticles != null)
        {
            ParticleSystem newParticle = Instantiate(muzzleFlashParticles, muzzleFlashPosition.position, Quaternion.identity);
            newParticle.transform.SetParent(muzzleFlashPosition);
        }
     
        OnGunShot?.Invoke();
        ammoManager.ConsumeAmmo(ammoType, ammoPerShot);
        StartCoroutine(camShake.Shake(shakeDuration, shakeMagnitude, rotationShakeMagnitude));
        currentCooldown = gunCooldown;
    }

    public void HitScanDamage()
    {
        int shots = isShotgun ? pelletsCount : 1;

        for (int i = 0;  i < pelletsCount; i++)
        {
            Vector3 shootDirection = playerCamera.forward;

            if (isShotgun)
            {
                shootDirection += playerCamera.right * Random.Range(-spreadAngle, spreadAngle) / 100f;
                shootDirection += playerCamera.up * Random.Range(-spreadAngle, spreadAngle) / 100f;
            }

            if (Physics.Raycast(playerCamera.position, shootDirection, out RaycastHit hit, hitscanRange, layerMask,  QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent(out Entity enemy))
                {
                    enemy.TakeDamage(gunDamage);
                }

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, -hit.normal);
                Instantiate(bulletHitWallParticles, hit.point, rot);
                
            }
        }  
    }

    public void ProjectileDamage()
    {
        if (projectilePrefab == null) return;

        Ray ray = playerController.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        int shots = isShotgun ? pelletsCount : 1;

        for (int i = 0; i < shots; i++)
        {
            Quaternion spreadRotation = projectileSpawnPoint.rotation;

            if (isShotgun)
            {
                spreadRotation *= Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0);
            }

            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, spreadRotation);

            if (newProjectile.TryGetComponent(out Projectile projScript))
            {
                projScript.projectileDamage = gunDamage;
                projScript.speed = projectileSpeed;
                projScript.explosionRadius = explosionRadius;
                projScript.projectileSize = projectileSize;

                if (!isShotgun)
                {
                   
                    if (Physics.Raycast(ray, out RaycastHit hit, hitscanRange))
                    {
                        projScript.AimDirection(hit.point);
                    }
                }
            }
        }
    }
}
