using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    [SerializeField]private int keys;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int GetPlayerKeys()
    {
        return keys;
    }

    public void ConsumeKey()
    {
        if (keys >= 0)
        {
            keys--;
            UIManager.instance.RefreshKeysUI(keys);
        }
    }

    public void AddKey()
    {
        keys++;
        UIManager.instance.RefreshKeysUI(keys);
    }
}
