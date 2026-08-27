using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public RoomManager rm;
    public int maxMonsters = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rm = FindFirstObjectByType<RoomManager>();

        int monsterNum = Random.Range(1, maxMonsters + 1);
        int monstersSpawned = 0;

        foreach(Transform child in transform.GetComponentsInChildren<Transform>())
        {
            // skip self
            if(child == transform)
                continue;

            Instantiate(monsterPrefab, child.position, Quaternion.identity);
            rm.enemies.Add(child.gameObject);
            monstersSpawned++;

            if(--monsterNum <= 0)
                break;
        }
        
        rm.enemiesLeft = monstersSpawned;
    }


}
