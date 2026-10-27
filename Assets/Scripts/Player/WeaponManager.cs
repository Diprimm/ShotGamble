using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [SerializeField] private List<GameObject> weapons;
    public List<bool> unlockedWeapons = new List<bool>();
    public int currentWeaponIndex = 0;
    public static event Action OnWeaponChanged;
    public Gun currentGun { get; private set; }
    public bool unlockAllWeapons; //Developer cheat

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        EquipWeapon(0);
    }
    private void Start()
    {
        

        if (unlockAllWeapons)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                unlockedWeapons.Add(true);
                weapons[i].SetActive(false);
            }

            EquipWeapon(0);

            return;
        }


        for (int i = 0; i < weapons.Count; i++)
        {
            //Se añade el arma inicial i como "true" al desbloqueo de armas.
            unlockedWeapons.Add(i == 0); //Añade true si el indice es 0, por lo que el arma 0 es true (desbloqueada).
            weapons[i].SetActive(false);
        }

        
    }
    
    private void Update()
    {
        //El index 9 es el Cat!!, el jugador no puede cambiar el arma mientras lo tiene equipado
        if (currentWeaponIndex == 9)
        {
            return;
        }

        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            //Si el scroll es positivo.
            if (scroll > 0)
            {
                //Se le suma 1 al index de cambio de arma.
                ChangeWeapon(1);
            }

            //Si el scroll es negativo.
            else if (scroll < 0)
            {
                //Se le resta 1 al index de cambio de arma.
                ChangeWeapon(-1);
            }
        }

    }

    public void UnlockWeapon(int index)
    {
        //Comprobación de limites de la lista para evitar errores y crashes.
        if (index >= 0 && index < unlockedWeapons.Count)
        {
            //Desbloquea el arma recibida por parámetro.
            unlockedWeapons[index] = true;
            EquipWeapon(index);
        }
    }

    public void ChangeWeapon(int direction)
    {
        int nextIndex = currentWeaponIndex;

        do
        {
            //Se busca la siguiente arma en la direccion del scroll.
            nextIndex = (nextIndex + direction + weapons.Count) % weapons.Count;
        }

        //Si el arma no está desbloqueada, pasará a la siguiente hasta que la condicion sea true.
        while (!unlockedWeapons[nextIndex]);

        //Cuendo sea true, se equipará el arma encontrada
        EquipWeapon(nextIndex);
    }

    private void EquipWeapon(int index)
    {      
        weapons[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].SetActive(true);

        currentGun = weapons[currentWeaponIndex].GetComponent<Gun>();

        OnWeaponChanged?.Invoke();

        if (currentWeaponIndex == 9)
        {
            UIManager.instance.RefreshObjectiveUI("Bring the cat to the exit", Color.white);
        }

        else
        {
            UIManager.instance.RefreshObjectiveUI("Find the cat", Color.white);
        }
    }
}
