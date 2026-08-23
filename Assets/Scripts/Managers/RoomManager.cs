using System.Collections.Generic;
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
    public int enemiesLeft;
    private int roomOffset = 0;

    public InputAction killEnemyDebug;
    public InputAction newRoomDebug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (camSpawnPoint != null)
            cam.transform.position = camSpawnPoint.transform.position;
        killEnemyDebug.Enable();
        newRoomDebug.Enable();
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

        GameObject newRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], new Vector3(roomOffset += 60, 0, 0), Quaternion.identity);
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
