// Este Script establece la posicion inicial del jugador al cargar una escena

using UnityEngine;

public class SetPlayerPosition : MonoBehaviour
{   
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false; // Desactivar para mover
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            cc.enabled = true; // Reactivar
        }
    }

}
