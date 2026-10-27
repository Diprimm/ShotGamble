using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (WeaponManager.instance.currentWeaponIndex == 9)
        {
            LevelManager.instance.LevelComplete();
        }
    }
}
