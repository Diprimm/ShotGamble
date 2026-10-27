using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Wave")]
    public int lastWaveSpawned = -1;
    public int currentWave = 0;
    [SerializeField] private int spawnersToActivate = 10;
    [SerializeField] private int remainingEnemiesInWave;

    [Header("Rescues")]
    [SerializeField] private int currentRescues;
    [SerializeField] private int itemsPerWave;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemySpawner[] enemySpawner;
    [SerializeField] private GameObject[] guns;
    
    
    public Transform gunSpawnPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemySpawner = Object.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        if (currentWave > lastWaveSpawned)
        {
            StartWave(currentWave);          
        }
    }

    public void RescueCat()
    {
        currentRescues++;
        //itemsPerWave solo lo usamos si queremos que el jugador necesite recuperar mas gatos,
        //el juego por ahora solo usa 1.

        //Esta condicion comprueba que el jugador salvo la cantidad requeridas de gatos para continuar con las oleadas.
        //Si el resto de la division da 0 pues esta bien y continua
        if (currentRescues % itemsPerWave == 0)
        {
            currentWave = currentRescues / itemsPerWave;

            if (currentWave - 1 < guns.Length)
            {
                Instantiate(guns[currentWave - 1], gunSpawnPoint.position, gunSpawnPoint.rotation);
                StartWave(currentWave);
            }               
        }
    }

    public void EnemyDied()
    {
        remainingEnemiesInWave--;

        if (remainingEnemiesInWave < 0)
        {
            remainingEnemiesInWave = 0;
        }
    }

    public void StartWave(int waveIndex)
    {
        //Detecta si estan disponibles los spawnerss
        if (enemySpawner == null || enemySpawner.Length == 0)
        {
            Debug.LogWarning("No se encontraron spawners para iniciar la oleada.");
            return;
        }

        Debug.Log("Oleada: " +  waveIndex + " iniciada");

        //Se genera una lista donde almacenamos los spawners
        List<int> availableIndices = new List<int>();

        for (int i = 0; i < enemySpawner.Length; i++)
        {
            availableIndices.Add(i);
        }

        //Almacena la cantidad de spawners que hay actualmente
        int count = Mathf.Min(spawnersToActivate, enemySpawner.Length);

        //Se spawnea el enemigo random en un spawner random disponible
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableIndices.Count);
            int spawnerIndex = availableIndices[randomIndex];

            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            enemySpawner[spawnerIndex].SpawnEnemy(randomEnemy);

            availableIndices.RemoveAt(randomIndex);
        }

        //La lastWave se establece que fue esta
        lastWaveSpawned = waveIndex;
    }

    public void LevelComplete()
    {
        //asdasda
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        UIManager.instance.LevelCompleteScreen();
        Time.timeScale = 0;
    }

    public void LevelFailed()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        UIManager.instance.LevelFailedScreen();
        Time.timeScale = 0;
    }
}
