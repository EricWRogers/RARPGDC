using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    public GameObject currentDoor;
    public GameObject cam;
    public List<GameObject> rooms;
    public int enemiesLeft;
    private int roomOffset = 0;

    public InputAction killEnemyDebug;
    public InputAction newRoomDebug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDoor = GameObject.Find("Door");
        Destroy(GameObject.Find("PlayerSpawnPoint")); // destroy it when ur done with it
        killEnemyDebug.Enable();
        newRoomDebug.Enable();
    }

    public void KillEnemy(GameObject enemy = null) // Enemy dying should call this function and pass in it's own GameObject
    {
        if(enemy)
            Destroy(enemy);

        if(--enemiesLeft <= 0)
        {
            if(currentDoor == null)
                currentDoor = GameObject.Find("Door");

            currentDoor.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Randomly choose new room, place player at spawn, move camera, remove spawn marker, count enemies, destroy current door, find new door 
    public void NewRoom()
    {
        Destroy(GameObject.Find("Door"));

        Instantiate(rooms[Random.Range(0, rooms.Count)], new Vector3(roomOffset += 60, 0, 0), Quaternion.identity);
   
        player.transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
        Destroy(GameObject.Find("PlayerSpawnPoint"));

        cam.transform.position = new Vector3 (player.transform.position.x - 7, cam.transform.localPosition.y, player.transform.position.z);

        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;

        currentDoor = GameObject.Find("Door");
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
            Debug.Log("Killed enemy");
            KillEnemy();
        }
    }
}
