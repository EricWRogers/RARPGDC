using UnityEngine;

public class CurrentDoor : MonoBehaviour
{
    public RoomManager rm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rm = FindFirstObjectByType<RoomManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && rm.enemiesLeft <= 0)
        {
            rm.NewRoom();
        }
    }

    public void UnlockDoor()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
