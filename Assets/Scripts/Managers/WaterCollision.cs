using Unity.VisualScripting;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    public GameObject UIManager;
    private UIManager _UIScript;

    void Start()
    {
        UIManager = GameObject.Find("UI");
        if (UIManager != null)
            _UIScript = UIManager.GetComponent<UIManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimplePlayerController _playerScript = other.GetComponent<SimplePlayerController>();
            _playerScript.isNearWater = true;
            _UIScript.ShowFlipInd();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimplePlayerController _playerScript = other.GetComponent<SimplePlayerController>();
            _playerScript.isNearWater = false;
            _UIScript.HideFlipInd();
        }
    }
}