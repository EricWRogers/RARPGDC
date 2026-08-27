using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimplePlayerController _playerScript = other.GetComponent<SimplePlayerController>();
            _playerScript.isNearWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimplePlayerController _playerScript = other.GetComponent<SimplePlayerController>();
            _playerScript.isNearWater = false;
        }
    }
}