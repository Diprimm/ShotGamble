using System.Collections.Generic;
using UnityEngine;
using System;

public enum AmmoType { Bullets, Shells, Rockets, Energy }

public class AmmoManager : MonoBehaviour
{
    private Dictionary<AmmoType, int> ammoInventory = new Dictionary<AmmoType, int>();

    public static event Action OnAmmoChange;

    void Awake()
    {
        ammoInventory[AmmoType.Bullets] = 999;
        ammoInventory[AmmoType.Shells] = 999;
        ammoInventory[AmmoType.Rockets] = 999;
        ammoInventory[AmmoType.Energy] = 999;
    }

    public bool HasAmmo(AmmoType type, int amount)
    {
        //Pregunta si el tipo de municion existe en el diccionario.
        //Luego pregunta si tiene la cantidad suficiente para disparar el arma.
        return ammoInventory.ContainsKey(type) && ammoInventory[type] >= amount;
    }

    public void ConsumeAmmo(AmmoType type, int amount)
    {
        if (HasAmmo(type, amount))
        {
            ammoInventory[type] -= amount;
            //Debug.Log($"Municion restante de {type}: {ammoInventory[type]}");
            OnAmmoChange?.Invoke();
        }
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        //Si ya existe el tipo de municion en el inventario, lo suma.
        if (ammoInventory.ContainsKey(type))
        {
            ammoInventory[type] += amount;
        }

        //Si no existe, se añade una entrada nueva.
        //El diccionario automaticamente crea la nueva llave (type).
        else
        {
            ammoInventory[type] = amount;
        }
    }

    public int GetAmmoCount(AmmoType type)
    {
        if (ammoInventory.ContainsKey(type))
        {
            return ammoInventory[type];
        }
        return 0;
    }
}
