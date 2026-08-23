using UnityEngine;
using UnityEngine.UI;

public class HealthBr : MonoBehaviour
{
    public Slider healthSlider;
    
    public void SetHealth(int health) // set health value
    {
        healthSlider.value = health;
    }

    public void SetMaxHealth(int health) //set the max health
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
}
