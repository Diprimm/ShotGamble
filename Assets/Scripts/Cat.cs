using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject sceneCat;
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        spawnPosition = GameObject.Find("DropPosition").GetComponent<Transform>();
    }
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Instantiate(sceneCat, spawnPosition.position, Quaternion.identity);
            WeaponManager.instance.unlockedWeapons[9]=false;
            WeaponManager.instance.ChangeWeapon(1);
            gameObject.SetActive(false);

        }
    }
}
