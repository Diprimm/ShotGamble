using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    //public GameObject enemyType;

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(1, 2, 1));
    }

}
