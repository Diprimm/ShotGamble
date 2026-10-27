using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    public static PersistentPlayer instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Player "sobrevive" al cargar una escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Si ya existe un jugador (al volver de la dungeon), destruimos el duplicado
            Destroy(gameObject);
        }
    }
}
