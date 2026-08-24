using UnityEngine;
using UnityEngine.UI;

public class HealthBr : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject player;
    public SimplePlayerController playerCtrl;
    
    public void SetHealth() // set health value
    {
        healthSlider.value = playerCtrl.maxHealth;
    }

    void Update() //set the max health
    {
        healthSlider.maxValue = playerCtrl.maxHealth;
        healthSlider.value = playerCtrl.health;
    }
}
