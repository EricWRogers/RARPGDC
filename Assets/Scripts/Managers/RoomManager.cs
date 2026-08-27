using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerSpawnPoint;
    public GameObject currentDoor;
    public GameObject cam;
    public GameObject camSpawnPoint;
    public List<GameObject> rooms;
    public List<GameObject> enemies;
    public int enemiesLeft;
    private int roomOffset = 0;
    public bool inRoomWithWater = false;

    public InputAction killEnemyDebug;
    public InputAction newRoomDebug;

    public void Invert()
    {
        Debug.Log("Inverse Triggered!");

        foreach (GameObject enemy in enemies)
        {
            EnemyAI _enemyScript = enemy.GetComponent<EnemyAI>();
            _enemyScript.isInversed = !_enemyScript.isInversed;
            _enemyScript.SetInversion();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (camSpawnPoint != null)
            cam.transform.position = camSpawnPoint.transform.position;
        killEnemyDebug.Enable();
        newRoomDebug.Enable();

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }

    public void KillEnemy(GameObject enemy = null) // Enemy dying should call this function and pass in it's own GameObject
    {
        if(enemy)
            Destroy(enemy);
        if(--enemiesLeft <= 0)
        {
            if (currentDoor != null)
                currentDoor.GetComponent<CurrentDoor>().UnlockDoor();
        }
    }

    // Randomly choose new room, place player at spawn, move camera, remove spawn marker, count enemies, destroy current door, find new door 
    public void NewRoom()
    {
        GameObject nextRoom = rooms[Random.Range(0, rooms.Count)];
        if(nextRoom.name != "Room1Prefab") inRoomWithWater = true;
        GameObject newRoom = Instantiate(nextRoom, new Vector3(roomOffset += 60, 0, 0), Quaternion.identity);
        camSpawnPoint = newRoom.GetComponentInChildren<CamSpawnPoint>()?.gameObject;
        playerSpawnPoint = newRoom.GetComponentInChildren<PlayerSpawnPoint>(true)?.gameObject;
        currentDoor = newRoom.GetComponentInChildren<CurrentDoor>(true)?.gameObject;


        if (playerSpawnPoint == null)
        {
            Debug.LogError($"Room {newRoom.name} has no PlayerSpawnPoint.");
            return;
        }

        CharacterController playerController = player.GetComponent<CharacterController>();
        if (playerController != null)
            playerController.enabled = false;

        player.transform.position = playerSpawnPoint.transform.position;
        playerSpawnPoint.SetActive(false);
        Physics.SyncTransforms();

        if (playerController != null)
            playerController.enabled = true;
        
        // Big room free cam
        if(camSpawnPoint == null)
        {
            cam.transform.position = new Vector3 (player.transform.position.x, cam.transform.position.y, player.transform.position.z);
            cam.GetComponent<CamManager>().stopFollowing = false;
        }
        // Small room locked cam
        else
        {
            cam.transform.position = camSpawnPoint.transform.position;
            cam.GetComponent<CamManager>().stopFollowing = true;
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;

    }

    public void Update()
    {
        //Debug test functions
        if (newRoomDebug.WasPressedThisFrame())
        {
            NewRoom();
        }

        if (killEnemyDebug.WasPressedThisFrame())
        {
            //Debug.Log("Killed enemy");
            KillEnemy();
        }
    }
}
