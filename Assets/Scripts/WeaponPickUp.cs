using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float floatSpeed;
    [SerializeField] private int weaponIndex;
    private SphereCollider sphereCollider;

    Vector3 startPos;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        startPos = transform.position;
    }

    private void Update()
    {
        RotateObject();
    }

    private void OnEnable()
    {
        sphereCollider.enabled = false;
        Invoke("EnableCollider", 1);
    }

    private void EnableCollider()
    {
        sphereCollider.enabled = true;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WeaponManager arsenal))
        {
            if (weaponIndex == 9)
            {
                if (arsenal.unlockedWeapons[9] == true)
                {
                    Debug.Log("no puedes llevar 2 gatos");
                    return;
                }

                else
                {
                    arsenal.UnlockWeapon(weaponIndex);
                    Despawn();
                }
            }

            else
            {
                arsenal.UnlockWeapon(weaponIndex);
                Despawn();
            }

        }
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * floatSpeed) * 0.1f;
        transform.position = tempPos;
    }
}
